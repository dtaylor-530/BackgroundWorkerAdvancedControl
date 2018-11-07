using DynamicData;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAsyncWorker
{
    public interface  IQueue<S>: UtilityInterface.IService<KeyValuePair<ChangeReason, S>>//: UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason, IWorkerItem<S>>>
    {

        //Queue<IAsyncWorkerItem<S>> Queue { get; }

        //void Cancel();
        void Enqueue(S qitem);

        //S Dequeue();
    }

    public interface IAsyncQueue<S> : UtilityInterface.IService<KeyValuePair<ChangeReason, S>>//: UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason, IWorkerItem<S>>>
    {

        //Queue<IAsyncWorkerItem<S>> Queue { get; }

        //void Cancel();
        Task<bool> Enqueue(S qitem);

        //S Dequeue();
    }
}
