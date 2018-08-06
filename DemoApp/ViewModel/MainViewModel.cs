using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BackgroundWorkerControl;
using System.Diagnostics;
using BackgroundWorkerWrapper;
using System.ComponentModel.Custom.Generic;

namespace DemoApp
{

    public  class MainViewModel
    {

        //public BackgroundWorkerEx<WorkerArgument> Worker { get; set; }

        public event EventHandler<EventArgs> Clear;
        public event EventHandler<ProgressChangedEventArgs<WorkerArgument>> Progress;

        public WorkerViewModel<WorkerArgument> WorkerViewModel { get;}

        private static int iterations = 10;
        public int Iterations { get { return iterations; } }

        public MainViewModel()
        {
           var Worker = new BackgroundWorkerEx<WorkerArgument>(10, MainWork, PreliminaryWork,  onProgressChanged: _backgroundWorker_ProgressChanged, onFinished: _backgroundWorker_RunWorkerCompleted) ;
            WorkerViewModel = new WorkerViewModel<WorkerArgument>(Worker);
        
        }


        public WorkerArgument MainWork(int arg)
        {

            var x = new Random();
            var y = x.NextDouble();
            return new WorkerArgument() { BaseNumber = arg, Rand = y };
        }

        public  object PreliminaryWork() { return null; }





        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs<WorkerArgument> e)
        {
            //TimeSpan ts = (sender as BackgroundWorkerEx).TimeSpan;
            WorkerViewModel.State = null;

            Helper.CreateMessage(default(TimeSpan),e.Cancelled);

            Clear?.Invoke(this, e);
        }



        private void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs<WorkerArgument> e)
        {
            WorkerViewModel.Progress = e.ProgressPercentage;
            Progress?.Invoke(this, e);

        }


      
    }







    static class Helper
    {

        public static  void CreateMessage(TimeSpan ts, bool cancelled)
        {
            string elapsedTime = "RunTime " + string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

            // Validate if the process is cancelled
            if (cancelled)
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
        }

    }


}
