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
using SilverlightApp.Helpers;

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
            var main = VisualTreeHelpers.FindAncestor<MainPage>(this);
            main.Sorteio.IsChecked = true;
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
                this.PreencherParticipante(dialog.File);
            }
        }
        #endregion

        private void PreencherParticipante(FileInfo file)
        {
            // clear the participants list
            ParticipantsList.Items.Clear();

            // open the file
            using (StreamReader reader = new StreamReader(file.OpenRead(), true))
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

        public void LerParticipantes(string path)
        {
            var file = new FileInfo(path);
            if (file.Exists)
            {
                this.PreencherParticipante(file);
            }
        }


    }
}
