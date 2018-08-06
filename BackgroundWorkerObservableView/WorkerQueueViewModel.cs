using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorkerObservableView
{




    public class WorkerQueueViewModel
    {
        public ReadOnlyReactiveProperty<int> Count { get; }
        public ReadOnlyReactiveProperty<int> Progress { get; }
        public ReadOnlyReactiveProperty<int> Output { get; }
        public ReadOnlyReactiveProperty<long> Time { get; }
        public ReadOnlyReactiveProperty<int> Completed { get; }
        public ReadOnlyReactiveProperty<int> QueuedItems { get; }
        public ReactiveCommand<string> RunCommand { get; } = new ReactiveCommand<string>();
        public ReadOnlyReactiveProperty<int> CancellationTime { get; } 



        public WorkerQueueViewModel(
            IObservable<int> tasks, IObservable<int> progress,IObservable<int> output,
            IObservable<long>time,IObservable<int>result,IObservable<int> count , IObservable<int> cancellationTimes)
        {


            Progress = progress.ToReadOnlyReactiveProperty();
            Output = output.ToReadOnlyReactiveProperty();
            Time = time.ToReadOnlyReactiveProperty();
            Completed = result.ToReadOnlyReactiveProperty();
            Count = count.ToReadOnlyReactiveProperty();
            CancellationTime = cancellationTimes.ToReadOnlyReactiveProperty();
            QueuedItems = tasks.CombineLatest(count, (a, b) => a - b).ToReadOnlyReactiveProperty();

        }

    }
}
