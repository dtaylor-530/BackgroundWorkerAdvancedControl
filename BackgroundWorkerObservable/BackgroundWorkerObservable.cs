using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel.Custom.Generic;
using System.Reactive.Linq;

namespace BackgroundWorkerExtended
{

    /* BackgroundWorker is ...
 *   ... meant to model a single task that you'd want to perform in the background, on a thread pool thread.  
 *   async/await is a syntax for asynchronously awaiting on asynchronous operations. 
 *   Those operations may or may not use a thread pool thread or even use any other thread. So, they're apples and oranges.
 */

    public class BackgroundWorkerObservable<T> /*: INotifyPropertyChanged*/ /*where T : new()*/
    {

        public IObservable<T> Result { get; }

        public IObservable<Tuple<int,T>> Progress { get; }

        System.Threading.ManualResetEvent _busy;

        BackgroundWorker<WorkerArgument<T>, T, T> _backgroundWorker { get; } = new BackgroundWorker<WorkerArgument<T>, T, T>
        {
            WorkerSupportsCancellation = true
        };


        //private Stopwatch stopWatch;



        public BackgroundWorkerObservable(Func<int, T> mainMethod, IObservable<bool?> commands,IObservable<int> iterations,IObservable<int> delay)
        {
            _busy = new System.Threading.ManualResetEvent(true);

            var x = new DoWork<T>(_busy, mainMethod);

            _backgroundWorker.DoWork += new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>(x.DoWork_Handler);

            Progress = Observable
                .FromEventPattern<ProgressChangedEventArgs<T>>(
                h => _backgroundWorker.ProgressChanged += h, h => _backgroundWorker.ProgressChanged -= h)
                .Select(_ => Tuple.Create(_.EventArgs.ProgressPercentage, _.EventArgs.UserState));


            Result= Observable.FromEventPattern<RunWorkerCompletedEventArgs<T>>(
                h => _backgroundWorker.RunWorkerCompleted += h, h => _backgroundWorker.RunWorkerCompleted -= h).Select(_ => _.EventArgs.Result);

            
            commands
                .CombineLatest(iterations,(a,b)=>new { a, b })
                .CombineLatest(delay,(c,dlay)=>new { command=c.a,iteration=c.b, dlay })
                .Subscribe(_ =>
            {

                var state = Converter.Main(_.command);
                if (state == WorkerState.Paused)
                   Pause();
                else if (state == WorkerState.Running)
                    Process(_.iteration,_.dlay);
                else if (state == WorkerState.Stopped)
                {
                    Cancel();
                }
                else
                {
                    throw new ArgumentOutOfRangeException("argument should be nullable bool");
                }
            });

        }


        private void Process(int iteration, int delay)
        {
            // Start the worker if it isn't running
            if (!_backgroundWorker.IsBusy) _backgroundWorker.RunWorkerAsync(new WorkerArgument<T>{ Iterations=iteration, Delay=delay });
            // Unblock the worker 
            _busy.Set();

        }

        private void Pause()
        {
            // Block the worker
            _busy.Reset();

        }

        private void Cancel()
        {
            if (_backgroundWorker.IsBusy)
            {
                // Set CancellationPending property to true
                _backgroundWorker.CancelAsync();
                // Unblock worker so it can see that
                _busy.Set();
 
            }
        }



    }





    struct WorkerArgument<T>
    {
        public int Iterations { get; set; }
        public int Delay { get; set; }
        public T Object { get; set; }

    }


    class DoWork<T>
    {
        private ManualResetEvent _busy;
        private Func<int, T> _;


        private Func<int, T> _mainMethod { get; set; }
        private TimeSpan _timeout;
        private Func<T, Func<int, T>> _preliminaryMethod;


        public DoWork(ManualResetEvent evnt, Func<T, Func<int, T>> preliminaryMethod, TimeSpan timeout)
        {
            _busy = evnt;
            _preliminaryMethod = preliminaryMethod;

            _timeout = timeout;
        }


        public DoWork(ManualResetEvent evnt, Func<int, T> mainMethod, TimeSpan timeout)
        {
            _busy = evnt;
            _mainMethod = mainMethod;
            _timeout = timeout;
        }

        public DoWork(ManualResetEvent evnt,  Func<int, T> mainMethod)
        {
            _busy = evnt;
            _mainMethod = mainMethod;

        }



        /// <summary>
        /// Do Work Event Handler
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Do Work Event Arguments</param>
        public void DoWork_Handler(object sender, DoWorkEventArgs<WorkerArgument<T>, T> args)
        {
            // Casting the sender as BackgroundWorker
            BackgroundWorker<WorkerArgument<T>, T,T> worker = sender as BackgroundWorker<WorkerArgument<T>, T,T>;

            if(_timeout!=default(TimeSpan))
            Observable.Timer(_timeout).Subscribe(_ => worker.CancelAsync());

        
            if (_mainMethod == null)
                 _mainMethod = _preliminaryMethod?.Invoke(args.Argument.Object);
 

            worker.ReportProgress(0, default(T));

            if (worker.CancellationPending)
            {
                args.Cancel = true;
            }

            int iterations = args.Argument.Iterations;
            int delay = args.Argument.Delay;

            T newT = default(T);
            // Simple loop to show the progress
            for (int i = 1; i <= iterations; i++)
            {
                // Allows for pausing code
                _busy.WaitOne();


                newT = _mainMethod(i);


                // check each iteration if the process is scheduled for cancellation
                if (worker.CancellationPending)
                {
                    // Break the process and set the Cancel flag as true
                    args.Cancel = true;
                    args.Result = newT;
                    worker.ReportProgress((int)(((double)i / iterations) * 100), newT);
                    break;
                }
                else
                {

                    // Update the Progress Bar
                    //  worker.ReportProgress(i * 10);
                    worker.ReportProgress((int)(((double)i / iterations) * 100), newT);
                    // Sleep for 500ms to show the changes
                    if(delay>0)Thread.Sleep((int)delay);
                }
            }

            args.Result = newT;

        }

      
        //private ManualResetEvent _busy;



        ///// <summary>
        ///// Do Work Event Handler
        ///// </summary>
        ///// <param name="sender">Sender</param>
        ///// <param name="args">Do Work Event Arguments</param>
        //public void DoWork_Handler2(object sender, DoWorkEventArgs<int[],T> args)
        //{
        //    // Casting the sender as BackgroundWorker
        //    BackgroundWorker<int[], T, T> worker = sender as BackgroundWorker<int[], T, T>;

        //    if (_timeout != default(TimeSpan))
        //        Observable.Timer(_timeout).Subscribe(_ => worker.CancelAsync());

        //    var mainMethod = _preliminaryMethod(args.Result);

        //    worker.ReportProgress(0, default(T));

        //    if (worker.CancellationPending)
        //    {
        //        args.Cancel = true;
        //    }

        //    int iterations = args.Argument[0];
        //    int delay = args.Argument[1];
        //    // Simple loop to show the progress
        //    for (int i = 1; i <= iterations; i++)
        //    {
        //        // Allows for pausing code
        //        _busy.WaitOne();


        //        var newT = mainMethod(i);


        //        // Always check if the process was cancelled
        //        if (worker.CancellationPending)
        //        {
        //            // Break the process and set the Cancel flag as true
        //            args.Cancel = true;
        //            worker.ReportProgress(0, newT);
        //            break;
        //        }
        //        else
        //        {

        //            // Update the Progress Bar
        //            //  worker.ReportProgress(i * 10);
        //            worker.ReportProgress((int)(((double)i / iterations) * 100), newT);
        //            // Sleep for 500ms to show the changes
        //            Thread.Sleep((int)delay);
        //        }
        //    }

        //}
    }
}





