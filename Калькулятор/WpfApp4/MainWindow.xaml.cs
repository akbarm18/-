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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WpfApp4
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timerOff,timerOn;
        bool bl = true;
        public MainWindow()
        {
            InitializeComponent();
             DataContext = new ViewModels.CalculateViewModel();
        }
        void TimerOff()
        {
            timerOff = new DispatcherTimer();
            timerOff.Tick += TimerOff_Tick;
            timerOff.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timerOff.Start();
        }
        void TimerOn()
        {
            timerOn = new DispatcherTimer();
            timerOn.Tick += TimerOn_Tick;
            timerOn.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timerOn.Start();
        }
        bool gridHistoryBool=false;
        private void TimerOn_Tick(object sender, EventArgs e)
        {
            if (gridHistoryBool)
            {
                if (GridHistory.Height >= 320)
                {
                    timerOn.Stop();
                }
                else
                {
                    GridHistory.Height += 25;
                }
            }
            else
            {
                if (GridMenu.Width >= 250)
                {
                    timerOn.Stop();
                }
                else
                {
                    GridMenu.Width += 25;
                }
            }
            
        }

        private void TimerOff_Tick(object sender, EventArgs e)
        {
            if (gridHistoryBool)
            {
                if (GridHistory.Height <= 0)
                {
                    timerOff.Stop();
                    gridHistoryBool = false;
                }
                else
                {
                    GridHistory.Height -= 25;
                }
                
            }
            else
            {
                if (GridMenu.Width <= 0)
                {
                    timerOff.Stop();
                }
                else
                {
                    GridMenu.Width -= 25;
                }
            }
        }

        private void Zakrit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Svernut_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
     

        private void MenuOff_Click(object sender, RoutedEventArgs e)
        {
            TimerOff();
          
        }

        private void MenuOn_Click(object sender, RoutedEventArgs e)
        {
            TimerOn();
        }

        
        private void NewNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
        
        }
        private void Buttons_Click(object sender, RoutedEventArgs e)
        {
        }
        bool topmostbl;
        private void WindowTopMost_Click(object sender, RoutedEventArgs e)
        {
            if (topmostbl)
            {

            }
        }
        bool OnOf;
        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (OnOf)
            {
                gridHistoryBool = true;
                TimerOn();
                OnOf = !OnOf;
            }
            else
            {
                TimerOff();
                OnOf = !OnOf;
            }
            
        }

        private void Rasshirait_Click(object sender, RoutedEventArgs e)
        {
            if (bl)
            {
               
                this.WindowState = WindowState.Maximized;
                bl = false;
            }
            else
            {
                bl = true;
                this.WindowState = WindowState.Normal;
               
            }
            
        }
    }
}
