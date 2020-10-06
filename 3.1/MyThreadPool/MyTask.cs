using System;
using System.Threading;
using System.Threading.Tasks;

namespace MyThreadPool
{
    /// <summary>
    /// Implementation of IMyTask interface
    /// </summary>
    /// <typeparam name="TResult">Type of value of function in the task</typeparam>
    class MyTask<TResult> : IMyTask<TResult>
    {
        private readonly Func<TResult> func;
        private readonly MyThreadPool threadPool;
        private AggregateException aggregateException = null;
        private readonly ThreadSafetyQueue<Action> transfersToThreadPool = new ThreadSafetyQueue<Action>();

        /// <summary>
        /// Base construtor of tasks
        /// </summary>
        /// <param name="threadPool">Thread pool, where my tasks  executes</param>
        /// <param name="func"></param>
        public MyTask(MyThreadPool threadPool, Func<TResult> func)
        {
            this.func = func;
            this.threadPool = threadPool;
            Result = default(TResult);
        }

        /// <summary>
        /// Is task completed
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// Result of the calculation
        /// </summary>
        public TResult Result
        {
            get => aggregateException != null ? Result : throw aggregateException;
            private set => Result = value;
        }

        /// <summary>
        /// Starts execution
        /// </summary>
        public void Run()
        {
            try
            {
                Result = func();
            }
            catch (Exception e)
            {
                aggregateException = new AggregateException(e);
            }
            finally
            {
                while (!transfersToThreadPool.Empty)
                {
                    transfersToThreadPool.Dequeue().Invoke();
                }

                IsCompleted = true;
            }
        }

        /// <summary>
        /// Creates a new task by new function that gets result from this task
        /// </summary>
        /// <typeparam name="TNewResult">Type of the return value of the new function</typeparam>
        /// <param name="newFunc">New function</param>
        /// <returns>New task</returns>
        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newFunc)
        {
            if (newFunc == null)
            {
                throw new ArgumentNullException();
            }

            if (threadPool.cancellationTokenSource.IsCancellationRequested)
            {
                throw new ApplicationException();
            }

            var newTask = new MyTask<TNewResult>(threadPool, () => newFunc(Result));
            if (IsCompleted)
            {
                threadPool.Submit<TNewResult>(newTask);
            }
            else
            {
                transfersToThreadPool.Enqueue(() => threadPool.Submit<TNewResult>(newTask));
            }

            return newTask;
        }


    }

}
