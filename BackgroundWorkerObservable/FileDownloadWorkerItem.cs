//using FileDownloader;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace ReactiveAsyncWorker
{
    public class FileDownloadWorkerItem : FileDownloadWorkerItem<object>
    {
        public FileDownloadWorkerItem(Uri uri, string path, WebClient client) :base(uri,path,client)
        {
        }
    }


    public class FileDownloadWorkerItem<T>: ProgressWorkerItem
    {
        //private Uri _uri;
        private readonly string _path;

        public string Destination=>_path;

        public FileDownloadWorkerItem(Uri uri, string path, System.Net.WebClient client):
            base(
                client.GetProgress(), client.GetCompletion(),
                () => client.DownloadFileAsync(uri, path),uri.AbsolutePath)

        {
            _path = path;
            //Result = client.GetCompletion().TakeUntil(Completed).CombineLatest(Started, (a, b) => (T)a.UserState).ToReadOnlyReactiveProperty();

            //var progress = client.GetProgress().TakeUntil(Completed).CombineLatest(Started, (a, b) => a);

            //ProgressPercent = progress.Select(_ => _.ProgressPercentage).ToReadOnlyReactiveProperty();
            //Progress = progress.Select(_ => (T)_.UserState).ToReadOnlyReactiveProperty();

        }



    }



  

}
