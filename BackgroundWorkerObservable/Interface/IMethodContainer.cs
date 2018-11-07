using System;
using System.Collections.Generic;
using System.Text;

namespace ReactiveAsyncWorker
{
    public interface IMethodContainer
    {
        Func<int, KeyValuePair<IConvertible, object>> Method { get; }
    }

    public interface IMethodContainer<T>
    {
        T Method(int i);
        void PreliminaryMethod();
    }


    public abstract class MethodContainer<T> : IMethodContainer<T>
    {

        public abstract T Method(int i);

        public virtual void PreliminaryMethod()
        {

        }
    }
}
