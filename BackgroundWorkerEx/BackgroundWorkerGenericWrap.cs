//using System;
//using System.ComponentModel;
//using System.Globalization;
//using System.Threading;
//using System.Windows;
//using System.Windows.Input;
//using System.Diagnostics;
//using System.ComponentModel.Custom.Generic;

//namespace BackgroundWorkerWrapper
//{
//    /* BackgroundWorker is ...
// *   ... meant to model a single task that you'd want to perform in the background, on a thread pool thread.  
// *   async/await is a syntax for asynchronously awaiting on asynchronous operations. 
// *   Those operations may or may not use a thread pool thread or even use any other thread. So, they're apples and oranges.
// */


//    public class BackgroundWorkerEx<T> /*: INotifyPropertyChanged*/ where T : new()
//    {


//        public int Iteration { get; set; }

//        private Func<object> _preliminaryMethod;

//        private Func<int, T> _mainMethod;


//        //private TimeSpan elapsed;


//        public System.Threading.ManualResetEvent _busy { get; private set; }

//        BackgroundWorker<T> _backgroundWorker { get; } = new BackgroundWorker<T> { WorkerSupportsCancellation = true };


//        private Stopwatch stopWatch;

//        public BackgroundWorkerEx(int iteration, Func<int, T> mainMethod, Func<object> preliminaryMethod,
//            Action<object, ProgressChangedEventArgs<T>> onProgressChanged = null,
//               Action<object, RunWorkerCompletedEventArgs<T>> onFinished = null)
//        {
//            _mainMethod = mainMethod;
//            _preliminaryMethod = preliminaryMethod;

//            Action<object, DoWorkEventArgs<T>> doWork = DoWork_Handler;
//            Iteration = iteration;
//            Initialise(doWork, onProgressChanged, onFinished);
//        }



//        public BackgroundWorkerEx(Action<object, DoWorkEventArgs<T>> doWork,
//    Action<object, ProgressChangedEventArgs<T>> onProgressChanged = null,
//       Action<object, RunWorkerCompletedEventArgs<T>> onFinished = null)
//        {

//            Initialise(doWork, onProgressChanged, onFinished);
//        }



//        public void Initialise(
//            Action<object, DoWorkEventArgs<T>> doWork,
//            Action<object, ProgressChangedEventArgs<T>> onProgressChanged = null,
//               Action<object, RunWorkerCompletedEventArgs<T>> onFinished = null)
//        {


//            _busy = new System.Threading.ManualResetEvent(true);


//            // Connect handlers to BackgroundWorker object.
//            _backgroundWorker.DoWork += new EventHandler<DoWorkEventArgs<T>>(doWork);

//            if (onProgressChanged != null)
//                _backgroundWorker.ProgressChanged += new EventHandler<ProgressChangedEventArgs<T>>(onProgressChanged);

//            //_backgroundWorker.ProgressChanged += ProgressChanged_Handler;

//            if (onFinished != null)
//                _backgroundWorker.RunWorkerCompleted += new EventHandler<RunWorkerCompletedEventArgs<T>>(onFinished);

//            //_backgroundWorker.RunWorkerCompleted += RunWorkerCompleted_Handler;

//            stopWatch = new Stopwatch();

//        }




//        public void Process()
//        {
//            // Start the worker if it isn't running
//            if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync(new T());
//            // Unblock the worker 
//            _busy.Set();
//            //starttimer
//            //stopWatch.Start();
//            var running = stopWatch.IsRunning;

//        }

//        public void Pause()
//        {
//            // _backgroundWorker.CancelAsync();
//            // Block the worker
//            _busy.Reset();
//            stopWatch.Stop();

//        }

//        public void Cancel()
//        {
//            if (_backgroundWorker.IsBusy)
//            {
//                // Set CancellationPending property to true
//                _backgroundWorker.CancelAsync();
//                // Unblock worker so it can see that
//                _busy.Set();
//                //elapsed = stopWatch.Elapsed;
//                //stopWatch.Reset();
//            }
//        }


//        double _delay = 0;
//        public void ChangeDelay(double delay)
//        {
//            _delay = delay;

//        }



//        /// <summary>
//        /// Do Work Event Handler
//        /// </summary>
//        /// <param name="sender">Sender</param>
//        /// <param name="args">Do Work Event Arguments</param>
//        private void DoWork_Handler(object sender, DoWorkEventArgs<T> args)
//        {
//            // Casting the sender as BackgroundWorker
//            BackgroundWorker<T> worker = sender as BackgroundWorker<T>;

//            //// Extract the argument. 
//            //int arg = (T)(args.Argument;


//            // Start the time-consuming operation.
//            _preliminaryMethod();


//            if (worker.CancellationPending)
//            {
//                args.Cancel = true;
//            }


//            // Simple loop to show the progress
//            for (int i = 1; i <= Iteration; i++)
//            {
//                // Allows for pausing code
//                _busy.WaitOne();


//                T newT = _mainMethod(i);


//                // Always check if the process was cancelled
//                if (worker.CancellationPending)
//                {
//                    // Break the process and set the Cancel flag as true
//                    args.Cancel = true;
//                    worker.ReportProgress(0, newT);
//                    break;
//                }
//                else
//                {

//                    // Update the Progress Bar
//                    //  worker.ReportProgress(i * 10);
//                    worker.ReportProgress((int)(((double)i / Iteration) * 100), newT);
//                    // Sleep for 500ms to show the changes
//                    Thread.Sleep((int)_delay);
//                }
//            }

//        }


//        ///// <summary>
//        ///// Progress Changed Event Handler
//        ///// </summary>
//        ///// <param name="sender">Sender</param>
//        ///// <param name="args">Progress Changed Event Arguments</param>
//        //private void ProgressChanged_Handler(object sender, ProgressChangedEventArgs<T> args)
//        //{
//        //    // Update the current progress value
//        //    CurrentProgress = args.ProgressPercentage;
//        //}



//        ///// <summary>
//        ///// Run Worker Completed Event Handler
//        ///// </summary>
//        ///// <param name="sender">Sender</param>
//        ///// <param name="args">Run Worker Completed Event Args</param>
//        //private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs<T> args)
//        //{
//        //    // Reset the Progress Bar
//        //    CurrentProgress = 0;
//        //    State = null;



//        //}

//        //#region propertychange

//        //public event PropertyChangedEventHandler PropertyChanged;


//        ///// <summary>
//        ///// Property Changed.
//        ///// </summary>
//        ///// <param name="propertyName">Property Name.</param>
//        //protected void OnPropertyChanged(string propertyName)
//        //{
//        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//        //}
//        //#endregion propertychange
//    }




//}





