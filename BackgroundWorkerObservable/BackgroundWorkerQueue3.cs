//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ComponentModel;
//using System.ComponentModel.Custom.Generic;

//namespace BackgroundWorkerExtended
//{
//    /// <summary>
//    /// This is for a non-generic background worker
//    /// http://pooyakhamooshi.blogspot.com/2009/11/create-thread-safe-queued.html
//    /// </summary>
//    public class QueuedBackgroundWorker3<T,K,R>
//    {
//        #region Constructors

//        BackgroundWorker bw;

//        public QueuedBackgroundWorker3(IObservable<Func<int, T>> mainMethod)
//        {
//            bw.RunWorkerCompleted += (sender, args) =>
//            {
//                Queue.Dequeue();
//                Initialise(Func < T, K > doWork)
//                lock (lck)
//                {
//                    if (Queue.Count > 0)
//                    {
//                        (this.Queue.Peek()).RunWorkerAsync(args.Result);
//                    }
//                }
//            };

//            mainmethods.Subscribe(
//                _ => Queue.Enqueue(new QueueItem3(bw));
                
//                )
//            bw = new BackgroundWorker();
//            bw.WorkerReportsProgress = true;
//            bw.WorkerSupportsCancellation = true;

//        }

//        #endregion


//        #region Properties

//        Queue<QueueItem3> Queue = new Queue<QueueItem3>();

//        object lck = new object();

//        #endregion


//        #region Methods

//        private void RunAsync(Func<T, K> doWork, T inputArgument)
//        {

//            Initialise(doWork);

//        }

//        private void Initialise(Func<T, K> doWork, R r)
//        {
//            var x = new DoWork<T>(null, null, _k, TimeSpan.FromSeconds(timeout));

//            bw.DoWork += (sender, args) =>
//            {
//                if (doWork != null)
//                {
//                    args.Result = new EventHandler<DoWorkEventArgs<int[], T>>(x.DoWork_Handler);
//                }
//            };


//        }

//        #endregion
//    }


//    class QueueItem3
//    {

//        //object Argument;
//        BackgroundWorker BackgroundWorker;


//        public QueueItem3(BackgroundWorker backgroundWorker)
//        {
//            this.BackgroundWorker = backgroundWorker;
//            //this.Argument = argument;
//        }


//        public void RunWorkerAsync(object o)
//        {
//            this.BackgroundWorker.RunWorkerAsync(o);
//        }


//    }


//}

////public partial class Example
////{
////    protected void Page_Load(object sender, EventArgs e)
////    {
////        var worker = new QueuedBackgroundWorker();

////        worker.RunAsync<int, int>(Calculate, 2);

////        worker.RunAsync<MultiplyArgument, int>(Multiply, new MultiplyArgument() { A = 1, B = 2 }, MultiplyCompleted);
////    }

////    private int Calculate(int a)
////    {
////        return a * 2;
////    }

////    private int Multiply(MultiplyArgument calculateArgument)
////    {
////        return calculateArgument.A * calculateArgument.B;
////    }

////    private void MultiplyCompleted(int result, Exception error)
////    {
////        Response.Write("worker completed, result: " + result.ToString());
////    }

////}

////public class MultiplyArgument
////{
////    public int A { get; set; }

////    public int B { get; set; }

////}
