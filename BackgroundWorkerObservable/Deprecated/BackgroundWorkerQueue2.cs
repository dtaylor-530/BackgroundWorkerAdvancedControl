using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReactiveAsyncWorker
{

    //Background Worker Queue in C#

    // Chandra Shekhar Joshi, 27 Aug 2015


    // Points of Interest
    //I was using the Backgroundworker for performing some operation on each checkbox selection of treeview and all selection have some dependency,
    //so i have to use some locking to avoid updating objects by different workers at same time, once i implemented this Queue of Workers i didn't require lock and handling of selection became seamless.

    public class CustomWorker : BackgroundWorker
    {
        public CustomWorker(object sender)
        {
            this.Sender = sender;
        }

        public object Sender { get; private set; }

        public static void QueueWorker(
                            Queue<CustomWorker> queue,
                            object item,
                            Action<object, DoWorkEventArgs> action,
                            Action<object, RunWorkerCompletedEventArgs> actionComplete,
                            Action<RunWorkerCompletedEventArgs> displayError,
                            Action<object, ProgressChangedEventArgs> progressChange)
        {
            if (queue == null)
                throw new ArgumentNullException("queue");

            using (var worker = new CustomWorker(item))
            {
                worker.WorkerReportsProgress = true;
                worker.WorkerSupportsCancellation = true;

                worker.ProgressChanged += (sender, args) =>
                {
                    progressChange.Invoke(sender, args);
                };

                worker.DoWork += (sender, args) =>
                {
                    action.Invoke(sender, args);
                };

                Action aa = () =>
                  {
                      var next = queue.Peek();
                      next.ReportProgress(0, "Performing operation...");
                      next.RunWorkerAsync(next.Sender);
                  };

                Action< object, RunWorkerCompletedEventArgs> f = (sender, args) =>
                {
                    actionComplete.Invoke(sender, args);
                    queue.Dequeue();
                    if (queue.Count > 0)
                        aa();
                    else
                        displayError.Invoke(args);
                };

                worker.RunWorkerCompleted += (a, b) => f(a,b);
                // Worker_RunWorkerCompleted;
                queue.Enqueue(worker);
                if (queue.Count == 1) aa();
    
            }
        }


    }















    //Usage of CustomWorker is as below, a empty object of Queue, sender and actions are passed.

    //QueueWorker function will update the queue by creating the background worker for each action and queue them for execution.

    //Hide Copy Code



    //  var workerQueue = new Queue<CustomWorker>();
    //private void SelectTreeViewCheckBox(object sender)
    //    {
    //        CustomWorker.QueueWorker(
    //            this.workerQueue,
    //            sender,
    //            (x, e) =>
    //            {
    //                    //// some custom do work logic.
    //                },
    //            (x, e) =>
    //            {
    //                    //// some custom completed logic.
    //                },
    //            (e) =>
    //            {
    //                    //// some custom display error logic.
    //                },
    //            (x, e) =>
    //            {
    //                    //// Progress change logic.
    //                    this.ProgressValue = e.ProgressPercentage;
    //                this.Status = e.UserState.ToString();
    //            });
    //    }
}


