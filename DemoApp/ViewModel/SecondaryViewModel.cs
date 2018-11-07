using ReactiveAsyncWorker;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DemoApp
{
    public class SecondaryViewModel : UtilityWpf.NPC
    {

        //public BackgroundWorkerObservableView.WorkerQueueViewModel WorkerQueueVM { get; }
        public ReactiveCommand<string> RunCommand { get; } = new ReactiveCommand<string>();

        public Tuple<Uri, string> NewItem { get; private set; }

        private const string sss = @"http://www.picture-newsletter.com/arctic/arctic-0";
        private const string sty = @"https://upload.wikimedia.org/wikipedia/commons/e/e9/Felis_silvestris_silvestris_small_gradual_decrease_of_quality.png";

        private const string path = "../../Data/arctic";

        public SecondaryViewModel()
        {

            Init();

        }

        private void Init()
        {
            var ydt = Observable.Interval(TimeSpan.FromSeconds(5)).StartWith(0).Take(4).Select(_ => Tuple.Create(new Uri(sss + _ + ".jpg"), path + _ + 1 + ".png"));

            ydt.SubscribeOnDispatcher().Subscribe(_ =>
            {
                //System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                Application.Current.Dispatcher.Invoke(() =>
               {
                   NewItem = _;
                   OnPropertyChanged(nameof(NewItem));
                   Console.WriteLine("newitem");
               });
            
            }, e =>
            Console.WriteLine(e.Message), () => { });
        }
    }
}
