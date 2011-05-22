using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Axelerate.Silverlight3D.Media.Media3D;

namespace SilverlightApp.Controls
{
	public class TagCloud : Canvas
	{
		private RotateTransform3D rotateTransform;
        private Func<double, AxisAngleRotation3D> rotateTransformLambda;
        private List<TagItem> tagBlocks;
        private string[] tags;
		private bool isLoaded;
		private int ticks;

		public TagItem TopItem;
		
		public TagCloud()
		{
			Loaded += TagCloud_Loaded;
			SizeChanged += TagCloud_SizeChanged;
		}

		void TagCloud_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (tags != null)
				RedrawTagItem();
		}

		public void SetTags(string[] list)
		{
			tags = list;
		    
            if (!isLoaded)
		    {
		        return;
		    }

		    Children.Clear();
		    FillTags();
		}

		void TagCloud_Loaded(object sender, RoutedEventArgs e)
		{
			if (!isLoaded)
			{
				if (tags != null)
					FillTags();

				rotateTransform = new RotateTransform3D
				                      {
				                          Rotation = new AxisAngleRotation3D(new Vector3D(1.0, 0.0, 0.0), 0)
				                      };

			    CompositionTarget.Rendering += CompositionTarget_Rendering;

				isLoaded = true;
			}
		}

		public void RandomRotate()
		{
		    ticks = 0;

			var random = new Random();
			double relativeX = random.Next(-50, 100);
			double relativeY = random.Next(-50, 100);

			rotateTransformLambda = x => new AxisAngleRotation3D(new Vector3D(relativeY, relativeX, 0), x);
			rotateTransform.Rotation = rotateTransformLambda(CalculateRotationAngle());
		}

		void CompositionTarget_Rendering(object sender, EventArgs e)
		{
			++ticks;

			if (((AxisAngleRotation3D)rotateTransform.Rotation).Angle > 0.05)
			{
				RotateBlocks();
			}
			else
			{
				if (TopItem != null)
				{
				    TopItem.SetAsWinner();
				}
			}
		}

		public double CalculateRotationAngle()
		{
			return Math.Pow(10, 1 - (Math.Pow(this.ticks / 20.0, 3) / 200));
		}

		private void RotateBlocks()
		{
			rotateTransform.Rotation = rotateTransformLambda(CalculateRotationAngle());

			foreach (var textBlock in tagBlocks)
			{
				Point3D newPoint;
				if (rotateTransform.TryTransform(textBlock.CenterPoint, out newPoint))
				{
					textBlock.CenterPoint = newPoint;

					if (TopItem == null || TopItem.CenterPoint.Z < newPoint.Z)
					{
						if (TopItem != null)
						{
							TopItem.SetAsTop(false);
						}

						TopItem = textBlock;
						TopItem.SetAsTop(true);
					}

					textBlock.Redraw(ActualWidth / 2, ActualHeight / 2);
				}
			}
		}

		private void FillTags()
		{
			tagBlocks = new List<TagItem>();

			var radius = ActualWidth / 3;
			var max = tags.Length;

			for (var i = 1; i < max + 1; i++)
			{
				var phi = Math.Acos(-1.0 + (2.0 * i - 1.0) / max);
				var theta = Math.Sqrt(max * Math.PI) * phi;
				var x = radius * Math.Cos(theta) * Math.Sin(phi);
				var y = radius * Math.Sin(theta) * Math.Sin(phi);
				var z = radius * Math.Cos(phi);

				var tag = new TagItem(x, y, z, tags[i - 1]);
				tag.Redraw(Width / 2, Height / 2);
				
                Children.Add(tag);
				tagBlocks.Add(tag);
			}
		}

		private void RedrawTagItem()
		{
			if (Children.Count <= 0)
			{
				return;
			}

			var radius = ActualWidth / 3;
			var max = tags.Length;

			for (var i = 1; i < max + 1; i++)
			{
				var phi = Math.Acos(-1.0 + (2.0 * i - 1.0) / max);
				var theta = Math.Sqrt(max * Math.PI) * phi;
				var x = radius * Math.Cos(theta) * Math.Sin(phi);
				var y = radius * Math.Sin(theta) * Math.Sin(phi);
				var z = radius * Math.Cos(phi);

                // Possible bug here!
                var tagItem = (TagItem)Children[i - 1];
				
                tagItem.CenterPoint = new Point3D(x, y, z);
				tagItem.Redraw(ActualWidth / 2, ActualHeight / 2);
			}
		}
	}
}
