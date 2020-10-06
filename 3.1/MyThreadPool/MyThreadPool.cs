using System;
using System.Threading;

namespace MyThreadPool
{
    public class MyThreadPool
    {
        public int ThreadCount { get; }

        private readonly Thread[] threads;

        private readonly ThreadSafetyQueue<Action> waintingTasks = new ThreadSafetyQueue<Action>();

        public readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public MyThreadPool(int numberOfThreads)
        {
            if (numberOfThreads <= 0)
            {
                throw new ArgumentOutOfRangeException("Number of threads must be natural number");
            }
            ThreadCount = numberOfThreads;
            threads = new Thread[numberOfThreads];
            for (int i = 0; i < numberOfThreads; ++i)
            {
                threads[i] = new Thread(() => Run(cancellationTokenSource.Token));
                threads[i].Start();
            }
        }

        private void Run(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    if (!waintingTasks.Empty)
                    {
                        var task = waintingTasks.Dequeue();
                        task.Invoke();
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    if (!waintingTasks.Empty)
                    {
                        var task = waintingTasks.Dequeue();
                        task.Invoke();
                    }
                }
            }
        }

        public void Shutdown() => cancellationTokenSource.Cancel();

        public bool Submit<TResult>(Func<TResult> callBack)
        {
            if (callBack == null)
            {
                throw new ArgumentNullException();
            }

            if (cancellationTokenSource.IsCancellationRequested)
            {
                throw new ApplicationException("Thread pool doesn't take new tasks");
            }

            var task = new MyTask<TResult>(this, callBack);
            waintingTasks.Enqueue(task.Run);
            return true;
        }

        public void Submit<TResult>(IMyTask<TResult> task)
        {
            if (task == null)
            {
                throw new ArgumentNullException();
            }


        }

        public void Dispose()
        {
            Shutdown();
            cancellationTokenSource.Dispose();
        }


    }
}
