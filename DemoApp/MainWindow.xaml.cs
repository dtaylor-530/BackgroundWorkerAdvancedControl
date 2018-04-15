using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace DemoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        private Random Randomer = new Random();
        private MainWindowViewModel windowViewModel;
        public MainWindow()
        {
            InitializeComponent();
            windowViewModel = new MainWindowViewModel(_backgroundWorker_RunWorkerCompleted, _backgroundWorker_ProgressChanged);

            this.DataContext = windowViewModel.BackgroundWorkerEx;

        }




        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TimeSpan ts = windowViewModel.BackgroundWorkerEx.TimeSpan;
          
 

            string elapsedTime = "RunTime " + string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,   ts.Milliseconds / 10);

            // Validate if the process is cancelled
            if (e.Cancelled)
            {
                // Tell the user that the process was cancelled
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Properties.Resources.ProcessCancelled) + elapsedTime,
                    string.Format(CultureInfo.CurrentCulture, Properties.Resources.ProcessCancelledTitle)
                    );
            }
            else
            {



                // Tell the user that the process completed normally
                MessageBox.Show(string.Format(CultureInfo.CurrentCulture, Properties.Resources.ProcessCompleted) + elapsedTime,
                    string.Format(CultureInfo.CurrentCulture, Properties.Resources.ProcessCompletedTitle)
                    );
            }



            MainCanvas.Children.Clear();

      
        }




        private void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            Firefly CurrentFirefly = new Firefly();
            //CurrentFirefly.Speed = Randomer.Next(1, 3);
            CurrentFirefly.Body = new Ellipse();
            CurrentFirefly.Body.Margin = new Thickness(Randomer.Next(10, (int)MainCanvas.ActualWidth - 10),
                                                       Randomer.Next(10, (int)MainCanvas.ActualHeight - 10),
                                                       0, 0);
            CurrentFirefly.Body.Fill = Brushes.Black;
            CurrentFirefly.Body.Height = MainCanvas.ActualHeight / 4;
            CurrentFirefly.Body.Width = 1.5 * CurrentFirefly.Body.Height;
            MainCanvas.Children.Add(CurrentFirefly.Body);



        }





    }







    class Firefly
    {
        public Ellipse Body { get; set; }
        //public int Speed { get; set; }
    }

}

