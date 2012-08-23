using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverlightApp.Controls;
using System.IO;

namespace SilverlightApp.Views
{
    public partial class Participantes : UserControl
    {
        public Participantes()
        {
            InitializeComponent();
        }

        #region Events
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

        private void Ler_Click(object sender, RoutedEventArgs e)
        {
            // allow the user to pick a .txt file
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Ficheiro de texto|*.txt";
            
            if (dialog.ShowDialog().Value)
            {
                // clear the participants list
                ParticipantsList.Items.Clear();

                // open the file
                using (StreamReader reader = new StreamReader(dialog.File.OpenRead(), true))
                {
                    // read each line of the file
                    string participant;
                    while ((participant = reader.ReadLine()) != null)
                    {
                        // validate that the participant is filled in
                        if (!string.IsNullOrWhiteSpace(participant))
                            ParticipantsList.Items.Add(participant.Trim());
                    }
                }
            }
        }
        #endregion
    }
}
