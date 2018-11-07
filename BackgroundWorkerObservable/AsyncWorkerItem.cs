using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAsyncWorker
{
    public class AsyncWorkerItem<T> :WorkerItem<T>, IAsync<T>
    {

        public Task<T> Task { get; }

        private static T x;

        public AsyncWorkerItem(Task<T> task, string key):base(async () => x = await task, key)
        {
            Task = task;

            //Completed.Subscribe(_ => Output = _);
        }

        //public override void Start(object s)
        //{
        //    Started.OnNext(s);
           
        //}

    }




}
