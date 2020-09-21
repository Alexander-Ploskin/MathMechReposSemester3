using System;
using System.Collections.Generic;
using System.Text;

namespace MyThreadPool
{
    interface IMyTask<T>
    {
        bool IsCompleted { get; }

        T Result { get; }

        Func<TResult, TNewResult> 
    }
}
