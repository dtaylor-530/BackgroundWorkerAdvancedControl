//using FileDownloader;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Custom.Generic;
using System.Net;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;

namespace ReactiveAsyncWorker
{
    public class ProgressWorkerItem : WorkerItem<object>
    {
        public ReadOnlyReactiveProperty<object> Result { get; }

        public ReadOnlyReactiveProperty<int> ProgressPercent { get; }

        public ReadOnlyReactiveProperty<object> Progress { get; }

        public ProgressWorkerItem(IObservable<ProgressChangedEventArgs> progress, IObservable<AsyncCompletedEventArgs> completion, Action actn, string key) : base(actn, key)
        {
            ProgressPercent = progress.Select(_ => _.ProgressPercentage).TakeUntil(Completed).CombineLatest(Started, (a, b) => a).ToReadOnlyReactiveProperty();

            Progress = progress.Select(_ => (object)_.UserState).TakeUntil(Completed).CombineLatest(Started, (a, b) => a).ToReadOnlyReactiveProperty();

            Result = completion.TakeUntil(Completed).CombineLatest(Started, (a, b) => (object)a.UserState).ToReadOnlyReactiveProperty();
        }
    }





    public class ProgressWorkerItem<T> : WorkerItem<T>
    {

        public ReadOnlyReactiveProperty<T> Result { get; }

        public ReadOnlyReactiveProperty<int> ProgressPercent { get; }

        public ReadOnlyReactiveProperty<T> Progress { get; }


        public ProgressWorkerItem(IObservable<ProgressChangedEventArgs<T>> progress, IObservable<AsyncCompletedEventArgs> completion,Action actn, string key) :base(actn,key)
        {

            ProgressPercent = progress.Select(_ => _.ProgressPercentage).TakeUntil(Completed).CombineLatest(Started, (a, b) => a).ToReadOnlyReactiveProperty();

            Progress = progress.Select(_ => _.UserState).TakeUntil(Completed).CombineLatest(Started, (a, b) => a).ToReadOnlyReactiveProperty();

            Result = completion.TakeUntil(Completed).CombineLatest(Started, (a, b) => (T)a.UserState).ToReadOnlyReactiveProperty();

            //Time = GetTime(backgroundWorker, wa.Timeout.Ticks, Result).CombineLatest(Started, (a, b) => a).TakeUntil(Completed).ToReadOnlyReactiveProperty();

        }



    }



  

}
