using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;

using System.Threading.Tasks;
using Reactive.Bindings;
using System.Reactive.Linq;
using ReactiveAsyncWorker;
using System.Reactive.Subjects;

namespace DemoApp
{


    public class MainViewModel: UtilityWpf.NPC
    {

        //public IObservable<IObservable<WorkerArgument<object>>> NewItems { get; }
        ISubject<int> _itemsCount = new Subject<int>();
        public WorkerArgument<object> NewItem { get;private set; }


    public MainViewModel()
        {

            RestartCommand = new UtilityWpf.RelayCommand(() => _itemsCount.OnNext(5));

            var dis = Application.Current.Dispatcher;
            var x = new System.Reactive.Concurrency.DispatcherScheduler(dis);

            GetNewItems(_itemsCount).Switch().ObserveOn(x).Subscribe(_ =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    NewItem = _;
                    OnPropertyChanged(nameof(NewItem));
                });
            });

            RestartCommand.Execute(5);

        }


        private static IObservable<IObservable<WorkerArgument<object>>> GetNewItems(IObservable<int> itemcounts)
        {

            return itemcounts.Select(_ => Observable.Generate<int, WorkerArgument<object>>(
     0,
     value => value < _,
     value => value + 1,
     value => new WorkerArgument<object>
     {

         Iterations = 4,
         Delay = 500,
         MethodContainer = new DummyMethodContainer(),
         Timeout = TimeSpan.FromSeconds(4),
     }, i => TimeSpan.FromSeconds(i)));
        }


        public ICommand RestartCommand { get; }
    }

  


}
