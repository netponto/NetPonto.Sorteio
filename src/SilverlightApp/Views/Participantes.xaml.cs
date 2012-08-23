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
            // validate there's text
            if (String.IsNullOrEmpty(NewParticipant.Text))
                return;

            // validate that the participant doesn't get duplicated
            var alreadyExists = ParticipantsList.Items.Where(p => string.Compare(p as string, NewParticipant.Text, StringComparison.CurrentCultureIgnoreCase) == 0).Count();
            if (alreadyExists > 0)
            {
                // TODO: should show some kind of message
                return;
            }

            // add new participant
            ParticipantsList.Items.Add(NewParticipant.Text);

            // clear textbox
            NewParticipant.Text = string.Empty;
        }

        private void Ler_Click(object sender, RoutedEventArgs e)
        {
            // allow the user to pick a .txt file
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Ficheiro de texto|*.txt";
            
            if (dialog.ShowDialog().Value)
            {
                this.PreencherParticipantes(dialog.File);
            }
        }
        #endregion

        private void PreencherParticipantes(FileInfo file)
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

        public void LerParticipantes()
        {
            // TODO: ler participantes no arranque
            // Silverlight não permite aceder diretamente aos ficheiros, poderá ser utilizado o IsolatedStorage
            // http://blogs.silverlight.net/blogs/msnow/archive/2008/07/16/tip-of-the-day-19-using-isolated-storage.aspx
            //var path = @"Data\Participantes.txt";
            //var file = new FileInfo(path);
            //if (file.Exists)
            //{
            //    this.PreencherParticipantes(file);
            //}
        }

        private void NewParticipant_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.AddParticipant_Click(null, null);
            }
        }


    }
}
