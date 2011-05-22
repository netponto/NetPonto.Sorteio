using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SilverlightApp.Controls;

namespace SilverlightApp
{
	public partial class MainPage : UserControl
	{
        static readonly string[] participantes =
            {
				"Bruno Lopes",
				"Bárbara Castilho",
				"Caio Proiete",
				"Dmitry Ossipov",
				"Henrry Pires",
				"João Manso",
				"Jorge Silva",
				"Nuno Gomes",
				"Paulo Correia",
				"Paulo Morgado",
				"Pedro Rosa",
				"Sara Silva",
				"Ricardo Alves",
				"Virgílio Raposo",
            };

		public MainPage()
		{
			InitializeComponent();

            ParticipantsList.FontSize = 20;

		    ParticipantsList.Text = String
		        .Join(Environment.NewLine, participantes);

			this.KeyDown += MainPage_KeyDown;
		}

		void MainPage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == Key.Escape)
			{
				if (Application.Current.IsRunningOutOfBrowser)
				{
					Application.Current.MainWindow.Close();
				}
			}
		}

		private void Sortear_Click(object sender, RoutedEventArgs e)
		{
			this.LayoutRoot.Children.Clear();
			this.LayoutRoot.RowDefinitions.Clear();

			var memberInput = new List<string>();
			
            TextReader reader = new StringReader(ParticipantsList.Text = String
		        .Join(Environment.NewLine, participantes));
			
            string line;
			while ((line = reader.ReadLine()) != null)
			{
				memberInput.Add(line);
			}

			this.LayoutRoot.Children.Add(new TagRandomizer(memberInput)
			                             	{   VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
			                             		HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
											});
			// Go full screen
			Application.Current.Host.Content.IsFullScreen = true;
        }
    }
}
