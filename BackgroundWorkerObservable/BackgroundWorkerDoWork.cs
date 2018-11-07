using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel.Custom.Generic;
using System.Reactive.Linq;

namespace ReactiveAsyncWorker
{



     class BackgroundWorkerDoWork
    {
        private ManualResetEvent _busy;

        public BackgroundWorkerDoWork(ManualResetEvent evnt)
        {
            _busy = evnt;

        }


        public void DoWork_Handler<T>(BackgroundWorker<WorkerArgument<T>, T, T> worker, DoWorkEventArgs<WorkerArgument<T>, T> args)
        {


            if (args.Argument.Timeout != default(TimeSpan))
                Observable.Timer(args.Argument.Timeout).Subscribe(_ => worker.CancelAsync());


            //if (_mainMethod == null)
            //    _mainMethod = _preliminaryMethod?.Invoke(args.Argument.MethodContainer.);
            args.Argument.MethodContainer.PreliminaryMethod();

            worker.ReportProgress(0, default(T));

            if (worker.CancellationPending)
            {
                args.Cancel = true;
            }

            int iterations = args.Argument.Iterations;
            long delay = args.Argument.Delay;

            T newT = default(T);
            // Simple loop to show the progress
            for (int i = 1; i <= iterations; i++)
            {
                // Allows for pausing code
                _busy.WaitOne();


                newT =args.Argument.MethodContainer.Method(i);


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
                    if (delay > 0) Thread.Sleep((int)(delay));
                }
            }

            args.Result = newT;
        }




    }

}
