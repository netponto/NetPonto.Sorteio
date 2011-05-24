using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace SilverlightApp.Controls
{
    public partial class TagRandomizer : UserControl
    {
        private int ticks = 0;
        private readonly List<string> members;
        private string selectedMember;

        public TagRandomizer(IEnumerable<string> memberInput)
        {
            InitializeComponent();

            var r = new Random();

            members = memberInput.OrderBy(b => r.Next()).ToList();
            selectedMember = string.Empty;

            Cloud.SetTags(members.ToArray());
        }

        private void RandomMembers()
        {
            var timer = new DispatcherTimer();

            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromSeconds(0);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            var random = new Random();
            ticks++;

            if (ticks < 4)
            {
                this.Cloud.RandomRotate();
                ((DispatcherTimer)sender).Interval = TimeSpan.FromMilliseconds(random.Next(1500, 2000));
            }
            else
            {
                this.ticks = 0;
                selectedMember = Cloud.TopItem.Content.ToString();
                ((DispatcherTimer)sender).Stop();
            }
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            if (selectedMember != string.Empty)
            {
                members.Remove(selectedMember);
                selectedMember = string.Empty;
                Cloud.SetTags(members.ToArray());
            }
            else
            {
                Cloud.SetTags(members.ToArray());
            }

            RandomMembers();
        }
    }
}
