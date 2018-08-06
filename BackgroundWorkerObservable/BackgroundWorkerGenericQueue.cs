using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.Custom.Generic;

namespace BackgroundWorkerExtended
{
    /// <summary>
    /// This is generic backgroundworker
    /// </summary>
    public class BackgroundWorkerGenericQueue<T,R,S>
    {

        Queue<Func<bool>> Queue = new Queue<Func<bool>>();

        object lck = new object();

        public delegate void WorkerCompletedDelegate<K>(K result, Exception error);

        public BackgroundWorkerGenericQueue() { }

        public void RunAsync(Func<T, S> doWork, T inputArgument, WorkerCompletedDelegate<S> workerCompleted=null)
        {

            BackgroundWorker<T,R,S> bw = GetBackgroundWorker(doWork, workerCompleted);

            Queue.Enqueue(()=>bw.RunWorkerAsync(inputArgument));

            lock (lck)
                if (Queue.Count == 1)   this.Queue.Peek();
    
       }

        private BackgroundWorker<T,R,S> GetBackgroundWorker(Func<T,S> doWork, WorkerCompletedDelegate<S> workerCompleted=null)
        {
            BackgroundWorker<T,R,S> bw = new BackgroundWorker<T,R,S>();
            bw.WorkerReportsProgress = false;
            bw.WorkerSupportsCancellation = false;

            bw.DoWork += (sender, args) =>
            {
                if (doWork != null)
                {
                    args.Result = doWork(args.Argument);
                }
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                workerCompleted?.Invoke(args.Result, args.Error);
                Queue.Dequeue();
                lock (lck)
                    if (Queue.Count > 0) this.Queue.Peek();
            };
            return bw;
        }

    }

 
   

}

