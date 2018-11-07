using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveAsyncWorker
{
    public interface IAsync<T>
    {
        Task<T> Task { get; }
    }



}
