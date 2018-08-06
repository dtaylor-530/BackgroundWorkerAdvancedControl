﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;


namespace BackgroundWorkerExtended
{
    /// <summary>
    /// This is for a non-generic background worker
    /// http://pooyakhamooshi.blogspot.com/2009/11/create-thread-safe-queued.html
    /// </summary>
    public class QueuedBackgroundWorker
    {
        #region Constructors

        public QueuedBackgroundWorker() { }

        #endregion

        #region Properties

        Queue<object> Queue = new Queue<object>();

        object lockingObject1 = new object();

        public delegate void WorkerCompletedDelegate<K>(K result, Exception error);

        #endregion

        #region Methods

        /// <summary>
        /// doWork is a method with one argument
        /// </summary>
        /// <typeparam name="T">is the type of the input parameter</typeparam>
        /// <typeparam name="K">is the type of the output result</typeparam>
        /// <param name="inputArgument"></param>
        /// <param name="doWork"></param>
        /// <param name="workerCompleted"></param>
        public void RunAsync<T, K>(Func<T, K> doWork, T inputArgument, WorkerCompletedDelegate<K> workerCompleted)
        {
            BackgroundWorker bw = GetBackgroundWorker(doWork, workerCompleted);

            Queue.Enqueue(new QueueItem(bw, inputArgument));

            lock (lockingObject1)
            {
                if (Queue.Count == 1)
                {
                    ((QueueItem)this.Queue.Peek()).RunWorkerAsync();
                }
            }
        }

        /// <summary>
        /// Use this method if you don't need to handle when the worker is completed
        /// </summary>
        /// <param name="doWork"></param>
        /// <param name="inputArgument"></param>
        public void RunAsync<T, K>(Func<T, K> doWork, T inputArgument)
        {
            RunAsync(doWork, inputArgument, null);
        }

        private BackgroundWorker GetBackgroundWorker<T, K>(Func<T, K> doWork, WorkerCompletedDelegate<K> workerCompleted)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.WorkerReportsProgress = false;
            bw.WorkerSupportsCancellation = false;

            bw.DoWork += (sender, args) =>
            {
                if (doWork != null)
                {
                    args.Result = (K)doWork((T)args.Argument);
                }
            };

            bw.RunWorkerCompleted += (sender, args) =>
            {
                if (workerCompleted != null)
                {
                    workerCompleted((K)args.Result, args.Error);
                }
                Queue.Dequeue();
                lock (lockingObject1)
                {
                    if (Queue.Count > 0)
                    {
                        ((QueueItem)this.Queue.Peek()).RunWorkerAsync();
                    }
                }
            };
            return bw;
        }

        #endregion
    }


    public class QueueItem
    {
        #region Constructors

        public QueueItem(BackgroundWorker backgroundWorker, object argument)
        {
            this.BackgroundWorker = backgroundWorker;
            this.Argument = argument;
        }

        #endregion

        #region Properties

        public object Argument { get; private set; }

        public BackgroundWorker BackgroundWorker { get; private set; }

        #endregion

        #region Methods

        public void RunWorkerAsync()
        {
            this.BackgroundWorker.RunWorkerAsync(this.Argument);
        }

        #endregion
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
