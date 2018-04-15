using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using BackgroundWorkerUserControl;
using System.Diagnostics;

namespace BackgroundWorkerUserControl
{
    public class BackgroundWorkerEx<T> :BackgroundWorker, INotifyPropertyChanged where T : new()
    {


        private State state;

        public State State
        {
            get
            {
                return state;
            }
            set
            {

                if (state == value)
                    return;
                else if (value == State.Paused)
                    Pause();
                else if (value == State.Running)
                    Process();
                else if (value == State.Stopped)
                    Cancel();

                state = value;
                OnPropertyChanged("State");
            }
        }


        // Get the elapsed time as a TimeSpan value.
        public TimeSpan TimeSpan { get { return stopWatch.Elapsed; } }




        //delay for the main method
        public double Delay
        {
            get;
            set;
        } = 2000;

        public System.Threading.ManualResetEvent _busy { get; private set; }
        public BackgroundWorker _backgroundWorker { get; private set; }
        private int _currentProgress;

        private Stopwatch stopWatch;

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Property Changed.
        /// </summary>
        /// <param name="propertyName">Property Name.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Initialises code in this part of the class
        /// </summary>
        /// 
        //public BackgroundWorkerEx<T>() where T:new()
        //    {

        //    }


        public BackgroundWorkerEx()
        {


        }


        public void Task(int iteration,Action<object, DoWorkEventArgs> doWork=null,
            Action<object, ProgressChangedEventArgs> onProgressChanged=null,
               Action<object,RunWorkerCompletedEventArgs> onFinished = null)
        {
            Iteration = iteration;

            _busy = new System.Threading.ManualResetEvent(true);

            // Create BackgroundWorker object.
            _backgroundWorker = new BackgroundWorker
            {

                // Set BackgroundWorker properties.
                WorkerReportsProgress = true,


                WorkerSupportsCancellation = true
            };

            // Connect handlers to BackgroundWorker object.
            if (doWork != null)
                _backgroundWorker.DoWork += new DoWorkEventHandler(doWork);
            else
                _backgroundWorker.DoWork += DoWork_Handler;

  
            _backgroundWorker.ProgressChanged += ProgressChanged_Handler;

            if (onProgressChanged != null)
                _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(onProgressChanged);


            if (onProgressChanged != null)
                _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(onFinished);



            _backgroundWorker.RunWorkerCompleted += RunWorkerCompleted_Handler;


            stopWatch = new Stopwatch();

        }




        void Process()
        {
            // Start the worker if it isn't running
            if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync(new T()); 
            // Unblock the worker 
            _busy.Set();
            //starttimer
            stopWatch.Start();

        }

        void Pause()
        {
            // _backgroundWorker.CancelAsync();
            // Block the worker
            _busy.Reset();
            stopWatch.Stop();

        }

        void Cancel()
        {
            if (_backgroundWorker.IsBusy)
            {
                // Set CancellationPending property to true
                _backgroundWorker.CancelAsync();
                // Unblock worker so it can see that
                _busy.Set();
                stopWatch.Reset();
            }
        }



        /// <summary>
        /// Process Command
        /// </summary>
        public ICommand ProcessCommand
        {
            get
            {
                // return the Process relay command
                return new RelayCommand(Process, CanExecuteProcess);
            }
        }

        /// <summary>
        /// Cancel Command
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                // return the Cancel relay command
                return new RelayCommand(Cancel, CanExecuteCancel);
            }
        }

        /// <summary>
        /// Current Progress
        /// </summary>
        public int CurrentProgress
        {
            get
            {
                // return current progress
                return _currentProgress;
            }
            set
            {
                // validate if the current progress is not equal with the given value
                if (_currentProgress != value)
                {
                    // Update the progress value and raise property changed event on current progress
                    _currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }

        ///// <summary>
        ///// Process
        ///// </summary>
        //private void Process()
        //{
        //    // Validate if the worker is not busy
        //    if (!_backgroundWorker.IsBusy)
        //    {
        //        // Run the Asychronous worker
        //        _backgroundWorker.RunWorkerAsync();
        //    }
        //}

        /// <summary>
        /// Determine if the Process can be Executed.
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteProcess()
        {
            return true;
        }

        ///// <summary>
        ///// Cancel
        ///// </summary>
        //private void Cancel()
        //{
        //    // Cancel the Asynchronous worker
        //    _backgroundWorker.CancelAsync();
        //}

        /// <summary>
        /// Determine if the Cancel can be Executed
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteCancel()
        {
            // Just return true for now
            return true;
        }

        /// <summary>
        /// Progress Changed Event Handler
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Progress Changed Event Arguments</param>
        private void ProgressChanged_Handler(object sender, ProgressChangedEventArgs args)
        {
            // Update the current progress value
            CurrentProgress = args.ProgressPercentage;


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

            // Result :  result of asynchronous operation.

            // Start the time-consuming operation.
            PreliminaryMethod();


            if (worker.CancellationPending)
            {
                args.Cancel = true;
            }


            // Simple loop to show the progress
            for (int i = 1; i <= Iteration; i++)
            {
                // Allows for pausing code
                _busy.WaitOne();


                T newT = MainMethod(i);


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
                    Thread.Sleep((int)Delay);
                }
            }

        }


        public int Iteration { get; set; }

        public Func<object> PreliminaryMethod { get; set; }

        public Func<int, T> MainMethod { get; set; }


        //private object PreliminaryWork(BackgroundWorker worker)
        //{
        //    // Simple loop to show the progress


        //        //// Always check if the process was cancelled
        //        //if (worker.CancellationPending)
        //        //{
        //        //    // Break the process and set the Cancel flag as true
        //        //   // args.Cancel = true;
        //        //    worker.ReportProgress(0);
        //        //    break;
        //        //}
        //        //else
        //        //{
        //        //    // Update the Progress Bar
        //        //    worker.ReportProgress(i * 10);
        //        //    //worker.ReportProgress((int)(((double)i / Events.Count()) * 100));
        //        //    // Sleep for 500ms to show the changes
        //        //    Thread.Sleep(500);
        //        //}
        //        Thread.Sleep(2000);


        //    return null;


        //}

        //private object MainWork(BackgroundWorker worker)
        //{
        //    // Simple loop to show the progress



        //        //// Always check if the process was cancelled
        //        //if (worker.CancellationPending)
        //        //{
        //        //    // Break the process and set the Cancel flag as true
        //        //   // args.Cancel = true;
        //        //    worker.ReportProgress(0);
        //        //    break;
        //        //}
        //        //else
        //        //{
        //        //    // Update the Progress Bar
        //        //    worker.ReportProgress(i * 10);
        //        //    //worker.ReportProgress((int)(((double)i / Events.Count()) * 100));
        //        //    // Sleep for 500ms to show the changes
        //        //    Thread.Sleep(500);
        //        //}
        //        Thread.Sleep(Delay);


        //    return null;


        //}


        /// <summary>
        /// Run Worker Completed Event Handler
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Run Worker Completed Event Args</param>
        private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
        {
            // Reset the Progress Bar
            CurrentProgress = 0;
            State = State.Stopped;



        }



        private void InitializeComponent()
        {
            // 
            // BackgroundWorkerEx
            // 
            this.WorkerReportsProgress = true;
            this.WorkerSupportsCancellation = true;

        }
    }






    /// <summary>
    /// Relay Command
    /// </summary>
    class RelayCommand : ICommand
    {
        #region Fields

        readonly Func<Boolean> _canExecute;
        readonly Action _execute;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="execute">Action</param>
        /// <param name="canExecute">Command State</param>
        public RelayCommand(Action execute, Func<Boolean> canExecute)
        {

            // set the action and command state from the given value
            _execute = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        #endregion

        #region ICommand Members

        /// <summary>
        /// Can Executed Changed Event Handler
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {

                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {

                if (_canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Can Execute
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Boolean CanExecute(Object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(Object parameter)
        {
            _execute();
        }

        #endregion
    }


}
