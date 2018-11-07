using System;
using System.Collections.Generic;
using System.ComponentModel.Custom.Generic;
using System.Reactive.Linq;
using System.Text;

namespace ReactiveAsyncWorker
{
    public static class BackgroundWorkerHelper
    {
        public static IObservable<RunWorkerCompletedEventArgs<T>> GetCompletion<T>(this BackgroundWorker<T> backgroundWorker)
        {
            return
           System.Reactive.Linq.Observable
                 .FromEventPattern<RunWorkerCompletedEventArgs<T>>(h => backgroundWorker.RunWorkerCompleted += h, h => backgroundWorker.RunWorkerCompleted -= h)
                 .Select(_ => _.EventArgs);
        }
        public static IObservable<RunWorkerCompletedEventArgs<T>> GetCompletion<T>(this BackgroundWorker<WorkerArgument<T>, T, T> backgroundWorker)
        {
            return
           System.Reactive.Linq.Observable
                 .FromEventPattern<RunWorkerCompletedEventArgs<T>>(h => backgroundWorker.RunWorkerCompleted += h, h => backgroundWorker.RunWorkerCompleted -= h)
                 .Select(_ => _.EventArgs);
        }

        public static IObservable<ProgressChangedEventArgs<T>> GetProgress<T>(this BackgroundWorker<WorkerArgument<T>, T, T> backgroundWorker)
    => Observable
            .FromEventPattern<ProgressChangedEventArgs<T>>(
            h => backgroundWorker.ProgressChanged += h, h => backgroundWorker.ProgressChanged -= h)
                        .Select(_ => _.EventArgs);

        public static IObservable<ProgressChangedEventArgs<T>> GetProgress<T>(this BackgroundWorker<T> backgroundWorker)
=> Observable
     .FromEventPattern<ProgressChangedEventArgs<T>>(
     h => backgroundWorker.ProgressChanged += h, h => backgroundWorker.ProgressChanged -= h)
                 .Select(_ => _.EventArgs);

        public static IObservable<long> GetTime<T>(this BackgroundWorker<WorkerArgument<T>, T, T> backgroundWorker, long timeout, IObservable<T> Result)
            => Observable.FromEventPattern<DoWorkEventArgs<WorkerArgument<T>, T>>(
               h => backgroundWorker.DoWork += h, h => backgroundWorker.DoWork -= h).Select(_ => _.EventArgs.Result)
               .Select(_ => SelectedTime(Result, timeout))
               .Switch();

        //public static IObservable<T> GetResult<T>(this BackgroundWorker<WorkerArgument<T>, T, T> backgroundWorker)
        //    => Observable.FromEventPattern<RunWorkerCompletedEventArgs<T>>(h => backgroundWorker.RunWorkerCompleted += h, h => backgroundWorker.RunWorkerCompleted -= h).Select(_ => _.EventArgs.Result)
        //    .Publish().RefCount();


        public static IObservable<long> SelectedTime<T>(this IObservable<T> results, long l) =>

             Observable.Timer(DateTime.Now, TimeSpan.FromMilliseconds(100))
                .TakeUntil(results.Select(_ => 0L)
                 .Amb(Observable.Interval(new TimeSpan(l))));



    }
}
