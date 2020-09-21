using System;
using System.Collections.Generic;
using System.Text;

namespace MyThreadPool
{
    class MyTask<TResult>
    {
        public bool IsCompleted { get; } = false;

        public TResult Result { get; } = default(TResult);

        public void ContinueWith(Type TNewResult, Func<TResult> func);


    }

}
