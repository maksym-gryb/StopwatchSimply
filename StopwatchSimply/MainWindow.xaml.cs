using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StopwatchSimply
{
    public partial class MainWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer clockDispatcherTimer;
        private TimeSpan timespan;
        private System.Windows.Threading.DispatcherTimer flashDispatcherTimer;

        private bool flashing = false;
        private bool flashing_when_paused = true;

        private DateTime since_last_tick;

        public MainWindow()
        {
            InitializeComponent();
            clockDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            clockDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            clockDispatcherTimer.Tick += myDispatcherTimer_Tick;

            timespan = TimeSpan.FromSeconds(0);

            this.Deactivated += Window_Deactivated;
            this.Topmost = true;

            flashDispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            flashDispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            flashDispatcherTimer.Tick += Flashing;
        }
        void myDispatcherTimer_Tick(object sender, EventArgs e)
        {
            timespan = timespan + (DateTime.Now - since_last_tick);
            since_last_tick = DateTime.Now;
            UpdateClock();
        }

        private void UpdateClock()
        {
            stopwatch_display.Text = String.Format("{0:00}:{1:00}:{2:00}", timespan.Hours, timespan.Minutes, timespan.Seconds);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                ContextMenu cm = this.FindResource("contextMenu") as ContextMenu;
                cm.PlacementTarget = sender as Button;
                cm.IsOpen = true;
            }
        }

        private void StartTimer(object sender, EventArgs e)
        {
            StartTimer();
        }

        private void StartTimer()
        {
            StopFlashing();
            clockDispatcherTimer.Start();
            stopwatch_display.Foreground = Brushes.Green;
            since_last_tick = DateTime.Now;
        }

        private void StopTimer(object sender, EventArgs e)
        {
            StopTimer();
        }

        private void StopTimer()
        {
            clockDispatcherTimer.Stop();
            stopwatch_display.Foreground = Brushes.White;
            StartFlashing();
        }

        private void ResetTimer(object sender, EventArgs e)
        {
            ResetTimer();
        }

        private void ResetTimer()
        {
            timespan = TimeSpan.FromSeconds(0);
            UpdateClock();
            StopFlashing();
        }

        public void OnKeyDownHandler(object sender, RoutedEventArgs e)
        {
            KeyEventArgs ke = e as KeyEventArgs;
            if (ke.Key == Key.Space)
            {
                ToggleTimer();
            }
            else if (ke.Key == Key.Escape)
            {
                Close();
            }
        }

        private void ToggleTimer()
        {
            if (clockDispatcherTimer.IsEnabled)
            {
                StopTimer();
            }
            else
            {
                StartTimer();
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            this.Topmost = true;
        }

        private void AlwaysOnTop_CheckedChanged(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;

            SetAlwaysOnTop(checkbox.IsChecked == true);
        }

        private void SetAlwaysOnTop(bool newVal)
        {
            this.Topmost = newVal;
        }

        private void Flashing(object sender, EventArgs e)
        {
            if (flashing_when_paused && flashing)
            {
                if (stopwatch_display.Foreground != Brushes.Red)
                {
                    stopwatch_display.Foreground = Brushes.Red;
                }
                else
                {
                    stopwatch_display.Foreground = Brushes.DarkRed;
                }
            }
        }

        private void StartFlashing()
        {
            flashing = true;
            stopwatch_display.Foreground = Brushes.Red;
            flashDispatcherTimer.Start();
        }

        private void StopFlashing()
        {
            flashDispatcherTimer.Stop();
            flashing = false;
            stopwatch_display.Foreground = Brushes.White;
        }

        private void FlashingWhenPausedToggle(object sender, EventArgs e)
        {
            var checkbox = sender as CheckBox;

            flashing_when_paused = checkbox.IsChecked ?? true;
        }

        private void ShowHelpWindow(object sender, EventArgs e)
        {
            ShowHelpWindow();
        }

        private void ShowHelpWindow()
        {
            HelpWindow helpWindow = new HelpWindow();
            helpWindow.Show();
        }
    }
}
