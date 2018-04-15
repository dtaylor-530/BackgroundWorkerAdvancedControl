﻿//using System;
//using System.ComponentModel;
//using System.Globalization;
//using System.Threading;
//using System.Windows;
//using System.Windows.Input;
//using BackgroundWorkerUserControl;
//using System.Diagnostics;


///* BackgroundWorker is ...
// *   ... meant to model a single task that you'd want to perform in the background, on a thread pool thread.  
// *   async/await is a syntax for asynchronously awaiting on asynchronous operations. 
// *   Those operations may or may not use a thread pool thread or even use any other thread. So, they're apples and oranges.
// */


//namespace DemoApp
//{

//    public partial class MainWindowViewModel
//    {

//        public MainWindowViewModel()
//        {
//            Init();
//        }

//        private void MainWork(int arg) { }

//        private void PreliminaryWork(int arg) { }

//    }




//    public partial class MainWindowViewModel: ViewModelBase
//    {


//        private State state;

//        public State State
//        {
//            get
//            {
//                return state;
//            }
//            set
//            {

//                if (state == value)
//                    return;
//                else if (value == State.Paused)
//                    Pause();
//                else if (value == State.Running)
//                    Process();
//                else if (value == State.Stopped)
//                    Cancel();

//                state = value;
//                OnPropertyChanged("State");
//            }
//        }


//        // Get the elapsed time as a TimeSpan value.
//        public TimeSpan TimeSpan { get { return stopWatch.Elapsed; } }




//        //delay for the main method
//        public int Delay { get; set; } = 1000;
//        public System.Threading.ManualResetEvent _busy { get; private set; }
//        public BackgroundWorker _backgroundWorker { get; private set; }
//        private int _currentProgress;

//        private Stopwatch stopWatch;

//        /// <summary>
//        /// Initialises code in this part of the class
//        /// </summary>
//        public void Init()
//        {
//            _busy = new System.Threading.ManualResetEvent(true);

//            // Create BackgroundWorker object.
//            _backgroundWorker = new BackgroundWorker();

//            // Set BackgroundWorker properties.
//            _backgroundWorker.WorkerReportsProgress = true;
//            _backgroundWorker.WorkerSupportsCancellation = true;

//            // Connect handlers to BackgroundWorker object.
//            _backgroundWorker.DoWork +=DoWork_Handler;
//            _backgroundWorker.ProgressChanged += ProgressChanged_Handler;
//            _backgroundWorker.RunWorkerCompleted += RunWorkerCompleted_Handler;


//            stopWatch = new Stopwatch();
          
//        }




//        void Process()
//        {
//            // Start the worker if it isn't running
//            //if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync(tempCicle);
//            if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync(5);
//            // Unblock the worker 
//            _busy.Set();
//            //starttimer
//            stopWatch.Start();

//        }

//        void Pause()
//        {
//            // _backgroundWorker.CancelAsync();
//            // Block the worker
//            _busy.Reset();
//            stopWatch.Stop();

//        }

//        void Cancel()
//        {
//            if (_backgroundWorker.IsBusy)
//            {
//                // Set CancellationPending property to true
//                _backgroundWorker.CancelAsync();
//                // Unblock worker so it can see that
//                _busy.Set();
//                stopWatch.Reset();
//            }
//        }



//        /// <summary>
//        /// Process Command
//        /// </summary>
//        public ICommand ProcessCommand
//        {
//            get
//            {
//                // return the Process relay command
//                return new RelayCommand(Process, CanExecuteProcess);
//            }
//        }

//        /// <summary>
//        /// Cancel Command
//        /// </summary>
//        public ICommand CancelCommand
//        {
//            get
//            {
//                // return the Cancel relay command
//                return new RelayCommand(Cancel, CanExecuteCancel);
//            }
//        }

//        /// <summary>
//        /// Current Progress
//        /// </summary>
//        public int CurrentProgress
//        {
//            get
//            {
//                // return current progress
//                return _currentProgress;
//            }
//            set
//            {
//                // validate if the current progress is not equal with the given value
//                if (_currentProgress != value)
//                {
//                    // Update the progress value and raise property changed event on current progress
//                    _currentProgress = value;
//                    OnPropertyChanged("CurrentProgress");
//                }
//            }
//        }

//        ///// <summary>
//        ///// Process
//        ///// </summary>
//        //private void Process()
//        //{
//        //    // Validate if the worker is not busy
//        //    if (!_backgroundWorker.IsBusy)
//        //    {
//        //        // Run the Asychronous worker
//        //        _backgroundWorker.RunWorkerAsync();
//        //    }
//        //}

//        /// <summary>
//        /// Determine if the Process can be Executed.
//        /// </summary>
//        /// <returns></returns>
//        private bool CanExecuteProcess()
//        {
//            return true;
//        }

//        ///// <summary>
//        ///// Cancel
//        ///// </summary>
//        //private void Cancel()
//        //{
//        //    // Cancel the Asynchronous worker
//        //    _backgroundWorker.CancelAsync();
//        //}

//        /// <summary>
//        /// Determine if the Cancel can be Executed
//        /// </summary>
//        /// <returns></returns>
//        private bool CanExecuteCancel()
//        {
//            // Just return true for now
//            return true;
//        }

//        /// <summary>
//        /// Progress Changed Event Handler
//        /// </summary>
//        /// <param name="sender">Sender</param>
//        /// <param name="args">Progress Changed Event Arguments</param>
//        private void ProgressChanged_Handler(object sender, ProgressChangedEventArgs args)
//        {
//            // Update the current progress value
//            CurrentProgress = args.ProgressPercentage;


//        }




//        /// <summary>
//        /// Do Work Event Handler
//        /// </summary>
//        /// <param name="sender">Sender</param>
//        /// <param name="args">Do Work Event Arguments</param>
//        private void DoWork_Handler(object sender, DoWorkEventArgs args)
//        {
//            // Casting the sender as BackgroundWorker
//            BackgroundWorker worker = sender as BackgroundWorker;



//            // Extract the argument. 
//            int arg = (int)args.Argument;

//            // Result :  result of asynchronous operation.

//            // Start the time-consuming operation.
//            PreliminaryWork(arg);


//            if (worker.CancellationPending)
//            {
//                args.Cancel = true;
//            }


//            // Simple loop to show the progress
//            for (int i = 1; i <= arg; i++)
//            {
//                // Allows for pausing code
//                _busy.WaitOne();


//                MainWork(arg);

//                // Always check if the process was cancelled
//                if (worker.CancellationPending)
//                {
//                    // Break the process and set the Cancel flag as true
//                    args.Cancel = true;
//                    worker.ReportProgress(0);
//                    break;
//                }
//                else
//                {

//                    // Update the Progress Bar
//                    //  worker.ReportProgress(i * 10);
//                    worker.ReportProgress((int)(((double)i / arg) * 100));
//                    // Sleep for 500ms to show the changes
//                    Thread.Sleep(Delay);
//                }
//            }

//        }


//        //public Func<int, object> PreliminaryWork { get; set; }

//        //public Func<int, object> MainWork { get; set; }
//        //private object PreliminaryWork(BackgroundWorker worker)
//        //{
//        //    // Simple loop to show the progress


//        //        //// Always check if the process was cancelled
//        //        //if (worker.CancellationPending)
//        //        //{
//        //        //    // Break the process and set the Cancel flag as true
//        //        //   // args.Cancel = true;
//        //        //    worker.ReportProgress(0);
//        //        //    break;
//        //        //}
//        //        //else
//        //        //{
//        //        //    // Update the Progress Bar
//        //        //    worker.ReportProgress(i * 10);
//        //        //    //worker.ReportProgress((int)(((double)i / Events.Count()) * 100));
//        //        //    // Sleep for 500ms to show the changes
//        //        //    Thread.Sleep(500);
//        //        //}
//        //        Thread.Sleep(2000);


//        //    return null;


//        //}

//        //private object MainWork(BackgroundWorker worker)
//        //{
//        //    // Simple loop to show the progress



//        //        //// Always check if the process was cancelled
//        //        //if (worker.CancellationPending)
//        //        //{
//        //        //    // Break the process and set the Cancel flag as true
//        //        //   // args.Cancel = true;
//        //        //    worker.ReportProgress(0);
//        //        //    break;
//        //        //}
//        //        //else
//        //        //{
//        //        //    // Update the Progress Bar
//        //        //    worker.ReportProgress(i * 10);
//        //        //    //worker.ReportProgress((int)(((double)i / Events.Count()) * 100));
//        //        //    // Sleep for 500ms to show the changes
//        //        //    Thread.Sleep(500);
//        //        //}
//        //        Thread.Sleep(Delay);


//        //    return null;


//        //}


//        /// <summary>
//        /// Run Worker Completed Event Handler
//        /// </summary>
//        /// <param name="sender">Sender</param>
//        /// <param name="args">Run Worker Completed Event Args</param>
//        private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
//        {
//            // Reset the Progress Bar
//            CurrentProgress = 0;
//           State = State.Stopped;

          
    
//        }

//        #region IDisposable Members

//        public void Dispose()
//        {
//            _backgroundWorker.Dispose();
//        }

//        #endregion
//    }






//    /// <summary>
//    /// Relay Command
//    /// </summary>
//    class RelayCommand : ICommand
//    {
//        #region Fields

//        readonly Func<Boolean> _canExecute;
//        readonly Action _execute;

//        #endregion

//        #region Constructors

//        /// <summary>
//        /// Constructor
//        /// </summary>
//        /// <param name="execute">Action</param>
//        /// <param name="canExecute">Command State</param>
//        public RelayCommand(Action execute, Func<Boolean> canExecute)
//        {
//            // Validate the given action value
//            if (execute == null)
//            {
//                // throw ArgumentNullException if the given action is null
//                throw new ArgumentNullException("execute");
//            }

//            // set the action and command state from the given value
//            _execute = execute;
//            _canExecute = canExecute;
//        }

//        #endregion

//        #region ICommand Members

//        /// <summary>
//        /// Can Executed Changed Event Handler
//        /// </summary>
//        public event EventHandler CanExecuteChanged
//        {
//            add
//            {

//                if (_canExecute != null)
//                {
//                    CommandManager.RequerySuggested += value;
//                }
//            }
//            remove
//            {

//                if (_canExecute != null)
//                {
//                    CommandManager.RequerySuggested -= value;
//                }
//            }
//        }

//        /// <summary>
//        /// Can Execute
//        /// </summary>
//        /// <param name="parameter"></param>
//        /// <returns></returns>
//        public Boolean CanExecute(Object parameter)
//        {
//            return _canExecute == null ? true : _canExecute();
//        }

//        /// <summary>
//        /// Execute
//        /// </summary>
//        /// <param name="parameter"></param>
//        public void Execute(Object parameter)
//        {
//            _execute();
//        }

//        #endregion
//    }

//}
