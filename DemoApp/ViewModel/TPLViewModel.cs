using ReactiveAsyncWorker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace DemoApp
{


    public class TPLViewModel : UtilityWpf.NPC
    {
        public Tuple<string, Task<string>> NewItem { get; private set; }

        public TPLViewModel(Dispatcher dispatcher)
        {
            var ydt = Observable.Interval(TimeSpan.FromSeconds(5))
                .StartWith(0).Take(3)
                .Select(_ => Tuple.Create<string, Task<string>>(_.ToString(), dse()));
            ydt.SubscribeOnDispatcher().Subscribe(_ =>
                {
                        NewItem = _;
                        OnPropertyChanged(nameof(NewItem));
                        Console.WriteLine("newitem");
                }, e =>
                Console.WriteLine(e.Message), () => { });

        }

        static HttpClient client = new HttpClient();

        private static Task<string> dse()
        {
            return client.GetStringAsync("https://msdn.microsoft.com");

        }

    }
}
