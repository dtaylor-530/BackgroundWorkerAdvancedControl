using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace BackgroundWorkerWrapper
{
    public class BackgroundWorkerWrap
    { 

        public int Iteration { get; set; }

        private Func<object> _preliminaryMethod;

        private Func<int, object> _mainMethod;


        //private TimeSpan elapsed;


        public System.Threading.ManualResetEvent _busy { get; private set; }

        BackgroundWorker _backgroundWorker { get; } = new BackgroundWorker { WorkerSupportsCancellation = true };


        private Stopwatch stopWatch;

        public BackgroundWorkerWrap(int iteration, Func<int, object> mainMethod, Func<object> preliminaryMethod,
            Action<object, ProgressChangedEventArgs> onProgressChanged = null,
               Action<object, RunWorkerCompletedEventArgs> onFinished = null)
        {
            _mainMethod = mainMethod;
            _preliminaryMethod = preliminaryMethod;

            Action<object, DoWorkEventArgs> doWork = DoWork_Handler;
            Iteration = iteration;
            Initialise(doWork, onProgressChanged, onFinished);
        }



        public BackgroundWorkerWrap(Action<object, DoWorkEventArgs> doWork,
    Action<object, ProgressChangedEventArgs> onProgressChanged = null,
       Action<object, RunWorkerCompletedEventArgs> onFinished = null)
        {

            Initialise(doWork, onProgressChanged, onFinished);
        }



        public void Initialise(
            Action<object,DoWorkEventArgs> doWork,
            Action<object, ProgressChangedEventArgs> onProgressChanged = null,
               Action<object, RunWorkerCompletedEventArgs> onFinished = null)
        {


            _busy = new System.Threading.ManualResetEvent(true);


            // Connect handlers to BackgroundWorker object.
            _backgroundWorker.DoWork += new DoWorkEventHandler(doWork);

            if (onProgressChanged != null)
                _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(onProgressChanged);

            //_backgroundWorker.ProgressChanged += ProgressChanged_Handler;

            if (onFinished != null)
                _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(onFinished);

            //_backgroundWorker.RunWorkerCompleted += RunWorkerCompleted_Handler;

            stopWatch = new Stopwatch();

        }

     
        public void Process()
        {
            // Start the worker if it isn't running
            if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync(null);
            // Unblock the worker 
            _busy.Set();
            //starttimer
            //stopWatch.Start();
            var running = stopWatch.IsRunning;

        }

        public void Pause()
        {
            // _backgroundWorker.CancelAsync();
            // Block the worker
            _busy.Reset();
            stopWatch.Stop();

        }

        public void Cancel()
        {
            if (_backgroundWorker.IsBusy)
            {
                // Set CancellationPending property to true
                _backgroundWorker.CancelAsync();
                // Unblock worker so it can see that
                _busy.Set();
                //elapsed = stopWatch.Elapsed;
                //stopWatch.Reset();
            }
        }


        double _delay = 0;
        public void ChangeDelay(double delay)
        {
            _delay = delay;

        }



        /// <summary>
        /// Do Work Event Handler
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Do Work Event Arguments</param>
        private void DoWork_Handler(object sender, DoWorkEventArgs args)
        {
            // Casting the sender as BackgroundWorker
            BackgroundWorker worker = sender as BackgroundWorker;

            //// Extract the argument. 
            //int arg = (T)(args.Argument;


            // Start the time-consuming operation.
            _preliminaryMethod();


            if (worker.CancellationPending)
            {
                args.Cancel = true;
            }


            // Simple loop to show the progress
            for (int i = 1; i <= Iteration; i++)
            {
                // Allows for pausing code
                _busy.WaitOne();


                object newT = _mainMethod(i);


                // Always check if the process was cancelled
                if (worker.CancellationPending)
                {
                    // Break the process and set the Cancel flag as true
                    args.Cancel = true;
                    worker.ReportProgress(0, newT);
                    break;
                }
                else
                {

                    // Update the Progress Bar
                    //  worker.ReportProgress(i * 10);
                    worker.ReportProgress((int)(((double)i / Iteration) * 100), newT);
                    // Sleep for 500ms to show the changes
                    System.Threading.Thread.Sleep((int)_delay);
                }
            }

        }

    }

}
