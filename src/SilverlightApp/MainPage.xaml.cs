using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using SilverlightApp.Controls;
using System.Text;

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

            //// ensure item list is empty before applying new source
            //ParticipantsList.Items.Clear();
            //// load the default participants to the list
            //_participantes.ForEach(x => ParticipantsList.Items.Add(x));

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
        #endregion

        private void Sorteio_Checked(object sender, RoutedEventArgs e)
        {
            if (this.Sorteio.IsChecked.HasValue && this.Sorteio.IsChecked.Value == true)
            {
                var memberInput = this.viewParticipantes.ParticipantsList.Items.Select(x => x.ToString());

                this.viewSorteio.LayoutRoot.Children.Add(new TagRandomizer(memberInput)
                {
                    VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch
                });

                // Go full screen
                //Application.Current.Host.Content.IsFullScreen = true;
            }
        }



    }
}
