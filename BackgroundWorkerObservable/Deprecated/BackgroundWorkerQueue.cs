using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;


namespace ReactiveAsyncWorker
{
    /// <summary>
    /// This is for a non-generic background worker
    /// http://pooyakhamooshi.blogspot.com/2009/11/create-thread-safe-queued.html
    /// </summary>
    public class QueuedBackgroundWorker
    {

        BackgroundWorker _bw;
        Queue<QueueItem> Queue = new Queue<QueueItem> ();

        object lockingObject = new object();

        public QueuedBackgroundWorker(BackgroundWorker bw=null)
        {
            _bw=bw?? new BackgroundWorker();


        }

        public void RunAsync(Func<object, object> doWork, object inputArgument, Action<object, Exception> workerCompleted)
        {
            //BackgroundWorker bw = GetBackgroundWorker(doWork, workerCompleted);

            Queue.Enqueue(new QueueItem(Queue,lockingObject,doWork, inputArgument,workerCompleted));

            lock (lockingObject)
            {
                if (Queue.Count == 1)
                {
                    (Queue.Peek()).RunWorkerAsync(_bw);
                }
            }
        }

        
    }


    public class QueueItem
    {

        object _lockingObject;

        private object _argument;

        private Func<object, object> _doWork;

        private Action<object, Exception> _workerComplete;

        Queue<QueueItem> _queue;

        public QueueItem(Queue<QueueItem> queue,object lockingObject, Func<object, object> doWork, object argument, Action<object, Exception> workerComplete)
        {
            _argument = argument;
            _lockingObject = lockingObject;
            _doWork = doWork;
            _workerComplete = workerComplete;
            _queue = queue;
        }



        public void RunWorkerAsync(BackgroundWorker bw)
        {
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.RunWorkerAsync();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs args)
        {
            if (_doWork != null)
            {
                args.Result = _doWork(args.Argument);
            }
        }

        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            var d = sender as BackgroundWorker;

            _workerComplete?.Invoke(args.Result, args.Error);

            _queue.Dequeue();
            lock (_lockingObject)
            {
                if (_queue.Count > 0)
                {
                    d.DoWork -= Bw_DoWork;
                    d.RunWorkerCompleted -= Bw_RunWorkerCompleted;
                    ((QueueItem)_queue.Peek()).RunWorkerAsync(d);
                }
            }

        }
    }


}

//public partial class Example
//{
//    protected void Page_Load(object sender, EventArgs e)
//    {
//        var worker = new QueuedBackgroundWorker();

//        worker.RunAsync<int, int>(Calculate, 2);

//        worker.RunAsync<MultiplyArgument, int>(Multiply, new MultiplyArgument() { A = 1, B = 2 }, MultiplyCompleted);
//    }

//    private int Calculate(int a)
//    {
//        return a * 2;
//    }

//    private int Multiply(MultiplyArgument calculateArgument)
//    {
//        return calculateArgument.A * calculateArgument.B;
//    }

//    private void MultiplyCompleted(int result, Exception error)
//    {
//        Response.Write("worker completed, result: " + result.ToString());
//    }

//}

//public class MultiplyArgument
//{
//    public int A { get; set; }

//    public int B { get; set; }

//}
