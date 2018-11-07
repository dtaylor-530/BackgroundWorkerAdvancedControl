using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.Custom.Generic;
using System.Collections.ObjectModel;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using DynamicData;
using System.Threading.Tasks;
using System.Reactive.Threading.Tasks;

namespace ReactiveAsyncWorker
{

    public abstract class AsyncWorkerQueue<S> : IQueue<IWorkerItem<S>>//, UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason, IWorkerItem<S>>>//IAsyncWorkerChanges<S>/*,INotifyPropertyChanged*/
    {
        protected object lockingObject = new object();

        Queue<IWorkerItem<S>> Queue = new Queue<IWorkerItem<S>>();

        public IObservable<KeyValuePair<ChangeReason,IWorkerItem<S>>> Resource { get; } = new Subject<KeyValuePair<DynamicData.ChangeReason, IWorkerItem<S>>>();


        public AsyncWorkerQueue(IObservable<S> obs)
        {
            obs.Subscribe( _=>
            {
                var s = Queue.Dequeue();
                s.Complete(_, new Exception());
  
                lock (lockingObject)
                {
                    if (Queue.Count > 0)
                        (Queue.Peek()).Start(null);
                }
                ( Resource as ISubject<KeyValuePair<DynamicData.ChangeReason, IWorkerItem<S>>>).OnNext(new KeyValuePair<ChangeReason, IWorkerItem<S>>(ChangeReason.Remove,s));

            });
        }


        public void Enqueue(IWorkerItem<S> qitem)
        {
            Queue.Enqueue(qitem);

            (Resource as ISubject<KeyValuePair<DynamicData.ChangeReason, IWorkerItem<S>>>).OnNext(new KeyValuePair<ChangeReason, IWorkerItem<S>>(ChangeReason.Add, qitem));

            lock (lockingObject)
            {
                if (Queue.Count == 1)
                    (Queue.Peek()).Start(null);
            }
        }
    }


}



