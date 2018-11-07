using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveAsyncWorker
{
    public struct WorkerArgument<T>
    {

        public int Iterations { get; set; }
        public long Delay { get; set; }
        public IMethodContainer<T> MethodContainer { get; set; }
      
        public TimeSpan Timeout { get; set; }
    }

}
