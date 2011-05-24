using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using Axelerate.Silverlight3D.Media.Media3D;

namespace SilverlightApp.Controls
{
    public class TagItem : Label
    {
        public TagItem(double x, double y, double z, string text)
        {
            BorderThickness = new Thickness(0, 0, 0, 0);
            BorderBrush = new SolidColorBrush(Colors.Black);
            Foreground = new SolidColorBrush(Color.FromArgb(255, 222, 194, 7));
            Padding = new Thickness(5, 5, 5, 5);
            FontFamily = new FontFamily("Segoe UI Bold");
            Content = text;
            CenterPoint = new Point3D(x, y, z);
        }

        public void Redraw(double xOffset, double yOffset)
        {
            var posZ = CenterPoint.Z + 200;

            this.FontSize = Math.Max(10 * (posZ / 100), 1);
            var alpha = CenterPoint.Z + 200;
            if (alpha > 255)
            {
                alpha = 255;
            }

            if (alpha < 0)
            {
                alpha = 0;
            }

            Foreground = new SolidColorBrush(Color.FromArgb(Convert.ToByte(alpha), 222, 194, 7));
            Canvas.SetLeft(this, CenterPoint.X + xOffset - (this.ActualWidth / 2));
            Canvas.SetTop(this, -CenterPoint.Y + yOffset - (this.ActualHeight / 2));
            Canvas.SetZIndex(this, Convert.ToInt32(CenterPoint.Z));
        }

        public void SetAsTop(bool isTop)
        {
            if (isTop)
            {
                BorderThickness = new Thickness(2, 2, 2, 2);
                Background = new SolidColorBrush(Color.FromArgb(75, 212, 207, 80));
                Foreground = new SolidColorBrush(Color.FromArgb(255, 222, 194, 7));
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 124, 96, 45));
            }
            else
            {
                BorderThickness = new Thickness(0, 0, 0, 0);
                Background = new SolidColorBrush(Colors.Transparent);
                Foreground = new SolidColorBrush(Color.FromArgb(255, 222, 194, 7));
            }
        }

        public string SetAsWinner()
        {
            BorderThickness = new Thickness(2, 2, 2, 2);
            Background = new SolidColorBrush(Color.FromArgb(75, 205, 212, 80));
            Foreground = new SolidColorBrush(Color.FromArgb(255, 229, 31, 31));
            BorderBrush = new SolidColorBrush(Color.FromArgb(255, 159, 31, 31));
            return Content.ToString();
        }

        public Point3D CenterPoint { get; set; }
    }
}
