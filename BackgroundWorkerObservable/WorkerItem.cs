using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace ReactiveAsyncWorker
{

    public class WorkerItem : WorkerItem<object>
    {
        public WorkerItem(Action action, string key) : base(action, key)
        {

        }
    }


    public class WorkerItem<T> : UtilityWpf.NPC, IWorkerItem<T>
    {

        public ISubject<T> Completed { get; } = new Subject<T>();
        public ISubject<object> Started { get; } = new Subject<object>();
        protected  Action _act { get; }

        public string Key { get; }
        public T Output { get; protected set; }

        public WorkerItem(Action action, string key)
        {
            _act = action;
            Key = key;
            Completed.Subscribe(_ => { Output = _; OnPropertyChanged(nameof(Output)); });
        }

        public virtual void Start(object s)
        {
            Started.OnNext(s);
            _act?.Invoke();
        }

        public virtual void Complete(T s, Exception exception)
        {
            Completed.OnNext(s);
        }
    }
}
