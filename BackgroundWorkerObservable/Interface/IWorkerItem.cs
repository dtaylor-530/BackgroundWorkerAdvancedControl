using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveAsyncWorker
{
    public interface IWorkerItem<S>
    {
        string Key { get; }

        void Start(object s);

        void Complete(S s, Exception exception);

        S Output { get; }
    }
}
