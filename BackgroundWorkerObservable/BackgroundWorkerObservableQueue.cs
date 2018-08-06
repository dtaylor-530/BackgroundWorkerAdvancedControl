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

namespace BackgroundWorkerExtended
{

    /* BackgroundWorker is ...
 *   ... meant to model a single task that you'd want to perform in the background, on a thread pool thread.  
 *   async/await is a syntax for asynchronously awaiting on asynchronous operations. 
 *   Those operations may or may not use a thread pool thread or even use any other thread. So, they're apples and oranges.
 */


    public class BackgroundWorkerObservableQueue<T> /*: INotifyPropertyChanged*/ //where T : new()
    {

        public IObservable<T> Result { get; }

        public IObservable<Tuple<int, T>> Progress { get; }

        public IObservable<long> Time { get; }

        public ISubject<int> Count { get; } = new Subject<int>();

        public ISubject<bool> Success { get; } = new Subject<bool>();

        //public Collections.Generic.Queue<Func<int, T>> Queue { get; } = new Collections.Generic.Queue<Func<int, T>>();


        System.Threading.ManualResetEvent _busy;

        BackgroundWorker<WorkerArgument<T>, T, T> _backgroundWorker { get; } = new BackgroundWorker<WorkerArgument<T>, T, T>
        {
            WorkerSupportsCancellation = true
        };




        public BackgroundWorkerObservableQueue(IObservable<Func<int, T>> mainMethod, IObservable<bool?> commands, IObservable<int> iterations, IObservable<int> delay, IObservable<int> timeout)
        {

            Progress = Observable
                  .FromEventPattern<ProgressChangedEventArgs<T>>(
                  h => _backgroundWorker.ProgressChanged += h, h => _backgroundWorker.ProgressChanged -= h)
                   .Select(_ => Tuple.Create(_.EventArgs.ProgressPercentage, _.EventArgs.UserState));

            Time = Observable.FromEventPattern<DoWorkEventArgs<WorkerArgument<T>, T>>(
               h => _backgroundWorker.DoWork += h, h => _backgroundWorker.DoWork -= h).Select(_ => _.EventArgs.Result)
               .CombineLatest(timeout, (a, b) => b)
               .Select(_ => Observable.Timer(DateTime.Now, TimeSpan.FromMilliseconds(1)).Take(_ * 1000)).Switch();

            Result = Observable.FromEventPattern<RunWorkerCompletedEventArgs<T>>(
                 h => _backgroundWorker.RunWorkerCompleted += h, h => _backgroundWorker.RunWorkerCompleted -= h).Select(_ => _.EventArgs.Result).Publish().RefCount();


            

            //Result.Subscribe(_=>  { Queue.Dequeue(); INotifyPropertyChanged(nameof();

            iterations.CombineLatest(delay, (a, b) => new { a, b })
                .CombineLatest(timeout, (a, b) => new { a, b })
                .Zip(commands, (d, c) => new { command = c, iteration = d.a.a, dlay = d.a.b, timeout = d.b })
                .Subscribe(_ =>
                {

                    var state = Converter.Main(_.command);
                    if (state == WorkerState.Paused)
                        Pause();
                    else if (state == WorkerState.Running)
                        Process(_.iteration, _.dlay, mainMethod, _.timeout);
                    else if (state == WorkerState.Stopped)
                    {
                        Cancel();
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("argument should be nullable bool");
                    }
                });
        }



        public BackgroundWorkerObservableQueue(IObservable<Func<T, Func<int, T>>> preliminaryMethod, T initial,IObservable<bool?> commands, IObservable<int> iterations, IObservable<int> delay, IObservable<int> timeout)
        {

            Progress = Observable
                  .FromEventPattern<ProgressChangedEventArgs<T>>(
                  h => _backgroundWorker.ProgressChanged += h, h => _backgroundWorker.ProgressChanged -= h)
                   .Select(_ => Tuple.Create(_.EventArgs.ProgressPercentage, _.EventArgs.UserState));

            Time = Observable.FromEventPattern<DoWorkEventArgs<WorkerArgument<T>, T>>(
               h => _backgroundWorker.DoWork += h, h => _backgroundWorker.DoWork -= h).Select(_ => _.EventArgs.Result)
               .CombineLatest(timeout, (a, b) => b)
               .Select(_ => Observable.Timer(DateTime.Now, TimeSpan.FromMilliseconds(1)).Take(_ * 1000)).Switch();

            Result = Observable.FromEventPattern<RunWorkerCompletedEventArgs<T>>(
                 h => _backgroundWorker.RunWorkerCompleted += h, h => _backgroundWorker.RunWorkerCompleted -= h).Select(_ => _.EventArgs.Result).Publish().RefCount();


            //Result.Subscribe(_=>  { Queue.Dequeue(); INotifyPropertyChanged(nameof();

            iterations.CombineLatest(delay, (a, b) => new { a, b })
                .CombineLatest(timeout, (a, b) => new { a, b })
                .Zip(commands, (d, c) => new { command = c, iteration = d.a.a, dlay = d.a.b, timeout = d.b })
                .Subscribe(_ =>
                {

                    var state = Converter.Main(_.command);
                    if (state == WorkerState.Paused)
                        Pause();
                    else if (state == WorkerState.Running)
                        Process(_.iteration, _.dlay, preliminaryMethod,initial, _.timeout);
                    else if (state == WorkerState.Stopped)
                    {
                        Cancel();
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException("argument should be nullable bool");
                    }
                });
        }



        private void Process(int iteration, int delay, IObservable<Func<int, T>> mainMethod, int timeout)
        {
            _busy = new System.Threading.ManualResetEvent(true);

            DoWork<T> x = null;
            int i = 0;

            //Func<Func<int,T> ,DoWork <T>> a =(_k)=> new DoWork<T>(_busy, _k, TimeSpan.FromSeconds(timeout));

            mainMethod.Subscribe(_k =>
            {
                if (x != null)
                    _backgroundWorker.DoWork -= new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>(x.DoWork_Handler);
                i++;
                Count.OnNext(i);

                x = new DoWork<T>(_busy, _k, TimeSpan.FromSeconds(timeout));

                _backgroundWorker.DoWork += new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>(x.DoWork_Handler);

                if (_backgroundWorker.IsBusy)
                    Success.OnNext(false);
                else
                {
                    _backgroundWorker.RunWorkerAsync(new WorkerArgument<T> { Iterations = iteration, Delay = delay });

                    // Unblock the worker 
                    _busy.Set();
                    Success.OnNext(true);
                }
                //_backgroundWorker.DoWork -= new EventHandler<DoWorkEventArgs<int[], T>>(x.DoWork_Handler);
            }, () => _backgroundWorker.DoWork -= new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>(x.DoWork_Handler));

        }



        private void Process(int iteration, int delay, IObservable<Func<T, Func<int, T>>> preliminaryMethod,T result, int timeout)
        {
            _busy = new System.Threading.ManualResetEvent(true);

            DoWork<T> x = null;
            int i = 0;

            //T result = default(T);

            Action<object, RunWorkerCompletedEventArgs<T>> fc = (a, b) => { result = b.Result==null? result:b.Result; };
            _backgroundWorker.RunWorkerCompleted += new EventHandler<RunWorkerCompletedEventArgs<T>>(fc);

            preliminaryMethod.Subscribe(_k =>
            {
                if (x != null)
                    _backgroundWorker.DoWork -= new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>(x.DoWork_Handler);
                i++;
                Count.OnNext(i);

                x = new DoWork<T>(_busy, _k, TimeSpan.FromSeconds(timeout));

                _backgroundWorker.DoWork += new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>(x.DoWork_Handler);


                if (!_backgroundWorker.IsBusy)
                {


                    //if ()
                    //throw new Exception("worker unexpectably busy");
                    // else
                    _backgroundWorker.RunWorkerAsync(new WorkerArgument<T> { Iterations = iteration, Delay = delay, Object = result });

                    // Unblock the worker 
                    _busy.Set();
                    Success.OnNext(true);
                }
                else
                    Success.OnNext(false);
                //_backgroundWorker.DoWork -= new EventHandler<DoWorkEventArgs<int[], T>>(x.DoWork_Handler);
            }, () =>
            {
                _backgroundWorker.DoWork -= new EventHandler<DoWorkEventArgs<WorkerArgument<T>, T>>(x.DoWork_Handler);
                _backgroundWorker.RunWorkerCompleted -= new EventHandler<RunWorkerCompletedEventArgs<T>>(fc);
            });

        }


        private void Pause()
        {
            // Block the worker
            _busy.Reset();

        }

        private void Cancel()
        {
            if (_backgroundWorker.IsBusy)
            {
                // Set CancellationPending property to true
                _backgroundWorker.CancelAsync();
                // Unblock worker so it can see that
                _busy.Set();

            }
        }



    }

}





