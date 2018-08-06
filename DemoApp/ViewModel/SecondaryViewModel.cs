using BackgroundWorkerExtended;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Custom.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoApp
{
    public class SecondaryViewModel
    {

        public BackgroundWorkerObservableView.WorkerQueueViewModel WorkerQueueVM { get; }
        public ReactiveCommand<string> RunCommand { get; } = new ReactiveCommand<string>();
        public int Iterations { get; } = 10;



        public SecondaryViewModel()
        {
            var CancellationTime = 3;

            Random r = new Random();

            Func<int, int> f = (i) => { System.Threading.Thread.Sleep(i * 300); return r.Next(100); };

            var yt = Observable.Interval(TimeSpan.FromSeconds(2)).Select(_ => f).Take(Iterations);

            var ydt = Observable.Repeat(20, 1);

            var u = yt.Scan(0, (a, b) => a + 1);
            var run = RunCommand.Select(_ => (bool?)bool.Parse(_));

            var ctimes = Observable.Repeat(CancellationTime, 1);
            var x = new BackgroundWorkerObservableQueue<int>(yt, run, ydt, run.Select(_ => 0), ctimes);

            var p = x.Progress.ObserveOnDispatcher().Publish().RefCount();

            var queuedItems = u.CombineLatest(x.Count, (a, b) => a - b).ToReadOnlyReactiveProperty();

            WorkerQueueVM = new BackgroundWorkerObservableView.WorkerQueueViewModel(
                u, p.Select(_ => _.Item1), p.Select(_ => _.Item2), x.Time,
               x.Result, x.Count, ctimes);


        }
    }
}
