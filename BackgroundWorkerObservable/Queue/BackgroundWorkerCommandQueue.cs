using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.ComponentModel.Custom.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Collections.Generic;
using UtilityEnum;
using Reactive.Bindings;
using UtilityInterface;

namespace ReactiveAsyncWorker
{

    public class BackgroundWorkerCommandQueue<T> : AsyncWorkerQueue<T> ,IPlayer/*: INotifyPropertyChanged*/ //where T : new()
    {
        private ManualResetEvent _busy;

        public ISubject<UtilityEnum.ProcessState> commands { get; } = new Subject<UtilityEnum.ProcessState>();

        static BackgroundWorker<WorkerArgument<T>, T, T> _backgroundWorker = new BackgroundWorker<WorkerArgument<T>, T, T>        {            WorkerSupportsCancellation = true, WorkerReportsProgress = true    };



        public BackgroundWorkerCommandQueue(IObservable<WorkerArgument<T>> mainMethod=null):base(_backgroundWorker.GetCompletion().Select(_=>_.Result))         
        {
            _busy = new System.Threading.ManualResetEvent(true);

            _backgroundWorker.DoWork += new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>((a, b) => { new BackgroundWorkerDoWork(_busy).DoWork_Handler(_backgroundWorker, b); });

            int i = 0;
            mainMethod?.Subscribe(_=>
            {
                i++;
                var x = new BackgroundWorkerItem<T>(_,i, _backgroundWorker);
                Enqueue(x);
            });

            React(commands);
        }



        private void React(IObservable<UtilityEnum.ProcessState> commands)
        {
            commands
            .Subscribe(command =>
            {
                if (command == UtilityEnum.ProcessState.Blocked)
                    Pause();
                //else if (command == UtilityEnum.ProcessState.Ready)
                //{
                //    _backgroundWorker.RunWorkerAsync(wa);
                //}
                else if (command == UtilityEnum.ProcessState.Running)
                    Resume();
                else if (command == UtilityEnum.ProcessState.Terminated)
                    Cancel();
                else
                    throw new ArgumentOutOfRangeException("argument should be nullable bool");
            }, () => _backgroundWorker.DoWork -= new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>((a, b) => { new BackgroundWorkerDoWork(_busy).DoWork_Handler(_backgroundWorker, b); }));
        }



        public  void Pause()
        {
            // Block the worker
            _busy.Reset();

        }

        public void Resume()
        {
            // UnBlock the worker
            _busy.Set();

        }

        public void Cancel()
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


}





