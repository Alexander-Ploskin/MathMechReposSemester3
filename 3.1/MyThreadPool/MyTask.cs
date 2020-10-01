using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyThreadPool
{
    class MyTask<TResult> : IMyTask<TResult>
    {
        private readonly Func<TResult> func;
        private readonly Mutex mutex = new Mutex();
        private readonly object lockObject = new Object();
        private readonly MyThreadPool threadPool;
        private TResult result = default(TResult);

        public MyTask(MyThreadPool threadPool, Func<TResult> func)
        {
            this.func = func;
            this.threadPool = threadPool;
        }

        public bool IsCompleted { get; private set; } = false;

        public TResult Result
        {
            get => IsCompleted ? result : default(TResult);
        }

        public void Run()
        {
            try
            {
                result = func();
            }
            catch (Exception e)
            {
                throw new Exception();
            }
            finally
            {

            }
        }

        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newFunc)
        {
            if (newFunc == null)
            {
                throw new ArgumentNullException();
            }
            var task = new MyTask<TNewResult>(threadPool, () => newFunc(result));

        }


    }

}
