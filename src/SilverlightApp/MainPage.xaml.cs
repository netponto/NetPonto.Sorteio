using System;
using System.Linq;
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
        #region Fields
        private List<string> _participantes = new List<string> 
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
        #endregion

        #region Constructors
        public MainPage()
        {
            InitializeComponent();

            // ensure item list is empty before applying new source
            ParticipantsList.Items.Clear();
            _participantes.ForEach(x => ParticipantsList.Items.Add(x));

            this.KeyDown += MainPage_KeyDown;
        }
        #endregion

        #region Events
        private void MainPage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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

            var memberInput = ParticipantsList.Items.Select(x => x.ToString());

            this.LayoutRoot.Children.Add(new TagRandomizer(memberInput) {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
            });

            // Go full screen
            Application.Current.Host.Content.IsFullScreen = true;
        }

        private void ParticipantsList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var wrapPanel = ParticipantsList.GetItemsHost() as WrapPanel;
            if (wrapPanel == null) return;

            wrapPanel.MaxHeight = ((FrameworkElement)sender).ActualHeight;
            wrapPanel.MaxWidth = ((FrameworkElement)sender).ActualWidth;
        }

        private void RemoveParticipant_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button;

            ParticipantsList.Items.Remove(b.CommandParameter);
        }

        private void AddParticipant_Click(object sender, RoutedEventArgs e)
        {
            // valdiate there's text
            if (String.IsNullOrEmpty(NewParticipant.Text))
                return;

            // add new participant
            ParticipantsList.Items.Add(NewParticipant.Text);
        }
        #endregion
    }
}
