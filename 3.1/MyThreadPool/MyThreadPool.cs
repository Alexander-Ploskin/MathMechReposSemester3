using System;
using System.Threading;
using System.Collections.Concurrent;

namespace MyThreadPoolRealisation
{
    /// <summary>
    /// Pool of threads that can execute tasks concurrently
    /// </summary>
    public class MyThreadPool
    {
        /// <summary>
        /// Threads in the pool
        /// </summary>
        private readonly Thread[] threads;

        /// <summary>
        /// Tasks, that is waiting to be executed
        /// </summary>
        private readonly BlockingCollection<Action> waitingTasks = new BlockingCollection<Action>();

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private readonly CountdownEvent countdownEventForShutdown;

        /// <summary>
        /// Creates the new pool with the fixed number of threads
        /// </summary>
        /// <param name="numberOfThreads">Number of threads in the new pool</param>
        public MyThreadPool(int numberOfThreads)
        {
            if (numberOfThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads must be natural number");
            }

            threads = new Thread[numberOfThreads];
            countdownEventForShutdown = new CountdownEvent(threads.Length);
            for (int i = 0; i < numberOfThreads; ++i)
            {
                threads[i] = new Thread(() => Run(cancellationTokenSource.Token));
                threads[i].Start();
            }
        }

        /// <summary>
        /// Working process of the thread pool
        /// </summary>
        /// <param name="cancellationToken">Token to shutdown work</param>
        private void Run(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    if (waitingTasks.TryTake(out var task))
                    {
                        task.Invoke();
                    }
                    else
                    {
                        countdownEventForShutdown.Signal();
                        break;
                    }
                }
                else
                {
                    Action task;
                    try
                    {
                        task = waitingTasks.Take(cancellationToken);
                        task?.Invoke();
                    }
                    catch (OperationCanceledException)
                    {
                        continue;
                    }
                }
            }
        }

        /// <summary>
        /// Shutdowns working of the pool
        /// Tasks, that already started doesn't terminate, but thread pool don't gets new tasks anymore
        /// </summary>
        public void Shutdown()
        {
            cancellationTokenSource.Cancel();
            countdownEventForShutdown.Wait();
        }


        /// <summary>
        /// Submits new function to thread pool
        /// </summary>
        /// <typeparam name="TResult">Type of the return value of the function</typeparam>
        /// <param name="newFunc">New function of the thread pool</param>
        /// <returns>Task by the new function</returns>
        public IMyTask<TResult> Submit<TResult>(Func<TResult> newFunc)
        {
            if (newFunc == null)
            {
                throw new ArgumentNullException(nameof(newFunc));
            }

            var task = new MyTask<TResult>(this, newFunc);
            try
            {
                waitingTasks.Add(task.Run, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                throw new ApplicationException("Thread pool isn't taking new tasks");
            }

            return task;
        }

        private void Submit<TResult>(MyTask<TResult> task)
        {
            try
            {
                waitingTasks.Add(task.Run, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException)
            {
                throw new ApplicationException("Thread pool isn't taking new tasks");
            }
        }

        /// <summary>
        /// Implementation of IMyTask interface
        /// </summary>
        /// <typeparam name="TResult">Type of value of function in the task</typeparam>
        private class MyTask<TResult> : IMyTask<TResult>
        {
            private Func<TResult> func;
            private readonly MyThreadPool threadPool;
            private AggregateException aggregateException = null;
            private readonly ConcurrentQueue<Action> transfersToThreadPool = new ConcurrentQueue<Action>();
            private readonly CountdownEvent waitResult = new CountdownEvent(1);

            /// <summary>
            /// Base construtor of tasks
            /// </summary>
            /// <param name="threadPool">Thread pool, where my tasks  executes</param>
            /// <param name="func">Func of the new task</param>
            public MyTask(MyThreadPool threadPool, Func<TResult> func)
            {
                this.func = func;
                this.threadPool = threadPool;
            }

            /// <summary>
            /// Is task completed
            /// </summary>
            public bool IsCompleted { get; private set; }

            private TResult result;

            /// <summary>
            /// Result of the calculation
            /// Blocks thread until result hasn't been ready
            /// </summary>
            public TResult Result
            {
                get
                {
                    if (aggregateException != null)
                    {
                        throw aggregateException;
                    }
                    waitResult.Wait();
                    return result;
                }
                private set => result = value;
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
                    while (transfersToThreadPool.TryDequeue(out var task))
                    {
                        task.Invoke();
                    }
                    func = null;
                    IsCompleted = true;
                    waitResult.Signal();
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
                    throw new ArgumentNullException(nameof(newFunc));
                }

                var newTask = new MyTask<TNewResult>(threadPool, () => newFunc(Result));
                if (IsCompleted)
                {
                    threadPool.Submit(newTask);
                }
                else
                {
                    transfersToThreadPool.Enqueue(() => threadPool.Submit(newTask));
                }

                return newTask;
            }

        }
    }
}
