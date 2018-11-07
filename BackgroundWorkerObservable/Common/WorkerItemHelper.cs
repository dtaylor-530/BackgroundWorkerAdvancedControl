using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAsyncWorker
{

    public static class WorkerItemServiceEx
    {
        public static async Task<T> GetItem<T>(this UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason, IWorkerItem<T>>> service, string key) 
        {
            //(Tasks as ISubject<T>).OnNext(kvp);

            return await service.Resource
                .Where(_ => _.Key == DynamicData.ChangeReason.Remove)
                .FirstAsync(i => i.Value.Key == key)
                .Select(i => i.Value.Output)
                .ToTask();

        }

        public static  IObservable<T> GetItem<S,T>(this UtilityInterface.IService<KeyValuePair<DynamicData.ChangeReason, S>> service, string key) where S:IWorkerItem<T>
        {
            //(Tasks as ISubject<T>).OnNext(kvp);

            return service.Resource
                .Where(_ => _.Key == DynamicData.ChangeReason.Remove)
                .FirstAsync(i => i.Value.Key == key)
                .Select(i => i.Value.Output);
               

        }
    }
}
