
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using DynamicData;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using System.Reactive.Subjects;
using UtilityInterface;
using System.Windows.Threading;

namespace ReactiveAsyncWorker
{

    public class TPLDataFlowQueue<T> : IAsyncQueue<WorkerItem<T>>
    {
        public IObservable<KeyValuePair<ChangeReason, WorkerItem<T>>> Resource { get; } = new Subject<KeyValuePair<ChangeReason,WorkerItem<T>>>();
        public ISubject<UtilityEnum.ProcessState> commands { get; } = new Subject<UtilityEnum.ProcessState>();
        private CancellationTokenSource _tokenSource;
        ActionBlock<Action> _block;
        //Dispatcher _dispatcher;


        public TPLDataFlowQueue(IObservable<KeyValuePair<string, Action>> tasks = null, CancellationTokenSource tokensource = null, TaskScheduler scheduler = null)
        {
            _tokenSource = tokensource ?? new CancellationTokenSource();
            var token = tokensource?.Token ?? new CancellationTokenSource().Token;

            _block = new ActionBlock<Action>(_ =>

            _(),
                new ExecutionDataflowBlockOptions()
                {
                    TaskScheduler = scheduler ?? TaskScheduler.Default,
                    //BoundedCapacity = 10000, // Cap the item count     
                    CancellationToken = token, // Enable cancellation
                    MaxDegreeOfParallelism = Environment.ProcessorCount, // Parallelize on all cores
                });

            tasks?
                .Select(_ =>
                new WorkerItem<T>(_.Value, _.Key))
                .Subscribe(async _ =>
                {

                    await Enqueue(_);

                }, (e) => { }/*, () => block.Complete()*/);
            React(commands);
        }




        public async Task<bool> Enqueue(WorkerItem<T> _)
        {
            var xx = new KeyValuePair<ChangeReason, WorkerItem<T>>(key: ChangeReason.Add, value: _);
            (Resource as ISubject<KeyValuePair<ChangeReason, WorkerItem<T>>>).OnNext(xx);
            //return await Task.Run(() =>
            //     {

            //_.Task.ToObservable().Subscribe(av =>
            //{
            //    _.Complete(av, null);
            //    xx = new KeyValuePair<ChangeReason, AsyncWorkerItem<T>>(key: ChangeReason.Remove, value: _);
            //    (Resource as ISubject<KeyValuePair<ChangeReason, AsyncWorkerItem<T>>>).OnNext(xx);
            //});

            return await _block.SendAsync(() =>/* _dispatcher.InvokeAsync*/_.Start(null));
        }


        //public IAsyncWorkerItem<T> Dequeue()
        //{
        //    throw new NotImplementedException();
        //}


        private void React(IObservable<UtilityEnum.ProcessState> commands)
        {
            commands
            .Subscribe(command =>
            {
                //if (command == UtilityEnum.ProcessState.Blocked)

                //else if (command == UtilityEnum.ProcessState.Ready)
                //{
                //    _backgroundWorker.RunWorkerAsync(wa);
                //}
                //else if (command == UtilityEnum.ProcessState.Running)

                if (command == UtilityEnum.ProcessState.Terminated)
                    Cancel();
                else
                    throw new ArgumentOutOfRangeException("argument should be nullable bool");
            });
        }



        public void Cancel()
        {
            _tokenSource.Cancel();
            // _block.Complete();
        }

    }
}


