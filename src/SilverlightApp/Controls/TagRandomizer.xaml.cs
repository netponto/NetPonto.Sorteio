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
        #region Fields
        private DispatcherTimer _timer;
        private int _ticks;
        private Random _random;
        private List<string> _members;
        #endregion

        #region Constructor
        public TagRandomizer(IEnumerable<string> memberInput)
        {
            InitializeComponent();

            _random = new Random();

            _members = memberInput.OrderBy(b => _random.Next())
                                  .ToList();

            _ticks = 0;

            _timer = new DispatcherTimer();
            _timer.Tick += Timer_Tick;
        }
        #endregion

        #region Methods
        private void RandomMembers()
        {
            _timer.Interval = TimeSpan.FromSeconds(0);
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _ticks++;

            if (_ticks < 4)
            {
                this.Cloud.RandomRotate();
                _timer.Interval = TimeSpan.FromMilliseconds(_random.Next(1500, 2000));
            }
            else
            {
                _timer.Stop();
                _ticks = 0;
            }
        }

        private void Go_Click(object sender, RoutedEventArgs e)
        {
            var selectedTag = Cloud.TopItem;
            if (selectedTag != null)
            {
                _members.Remove(selectedTag.Content.ToString());
            }

            Cloud.SetTags(_members.ToArray());
            RandomMembers();
        }
        #endregion
    }
}
