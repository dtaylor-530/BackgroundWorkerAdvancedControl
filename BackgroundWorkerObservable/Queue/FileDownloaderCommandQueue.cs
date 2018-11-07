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
using System.Net;
//using FileDownloader;

namespace ReactiveAsyncWorker
{


    public class FileDownloaderCommandQueue : AsyncWorkerQueue<object> /*: INotifyPropertyChanged*/ //where T : new()
    {

        public ISubject<UtilityEnum.ProcessState> commands { get; } = new Subject<UtilityEnum.ProcessState>();


        static WebClient _client = new WebClient();

        //static readonly IObservable<byte[]> obs = System.Reactive.Linq.Observable.FromEventPattern < DownloadDataCompletedEventHandler, DownloadDataCompletedEventArgs>(h => _client.DownloadDataCompleted+= h, h => _client.DownloadDataCompleted -= h).Select(_ => _.EventArgs.Result);

        public FileDownloaderCommandQueue(IObservable<Tuple<Uri, string>> mainMethod) : base(_client.GetCompletion().Select(_ => _.UserState))
        {

            mainMethod.Subscribe(_ =>
            {
                var x = new FileDownloadWorkerItem<object>(_.Item1, _.Item2, _client);
                Enqueue(x);
            },e=>
            Console.WriteLine(e.Message),()=> { });

            React(commands);
        }



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



        public  void Cancel()
        {
            if (_client.IsBusy)
                _client.CancelAsync();
        }


    }

    //public class FileDownloaderCommandQueue : AsyncWorkerQueue<CompletedState>,IPlayer /*: INotifyPropertyChanged*/ //where T : new()
    //{

    //    public ISubject<UtilityEnum.ProcessState> commands { get; } = new Subject<UtilityEnum.ProcessState>();


    //    static IFileDownloader fileDownloader = new FileDownloader.FileDownloader(new FileDownloader.DownloadCacheImplementation());

    //    Tuple<Uri, string> tus = default(Tuple<Uri, string>);

    //    public FileDownloaderCommandQueue(IObservable<Tuple<Uri, string>> mainMethod) : base(fileDownloader.GetCompletion().Select(_=>_.State))
    //    {

    //        int i = 0;

    //        mainMethod.Subscribe(_ =>
    //        {
    //            tus = _;
    //            var x = new FileDownloadWorkerItem<CompletedState>(_.Item1, _.Item2, fileDownloader);
    //            Run(x);
    //        });

    //        React(commands);
    //    }



    //    private void React(IObservable<UtilityEnum.ProcessState> commands)
    //    {
    //        commands
    //        .Subscribe(command =>
    //        {
    //            if (command == UtilityEnum.ProcessState.Blocked)
    //                Pause();
    //            //else if (command == UtilityEnum.ProcessState.Ready)
    //            //{
    //            //    _backgroundWorker.RunWorkerAsync(wa);
    //            //}
    //            else if (command == UtilityEnum.ProcessState.Running)
    //                Resume();
    //            else if (command == UtilityEnum.ProcessState.Terminated)
    //                Cancel();
    //            else
    //                throw new ArgumentOutOfRangeException("argument should be nullable bool");
    //        });
    //    }



    //    public void Cancel()
    //    {

    //        fileDownloader.CancelDownloadAsync();
    //    }

    //    public void Pause()
    //    {
    //        fileDownloader.CancelDownloadAsync();
    //    }

    //    public void Resume()
    //    {
    //        fileDownloader.DownloadFileAsync(tus.Item1,tus.Item2);
    //    }
    //}


}
