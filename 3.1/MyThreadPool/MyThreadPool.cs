using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyThreadPool
{
    public class MyThreadPool
    {
        public int ThreadCount { get; }

        private Thread[] threads;

        private Queue<IMyTask<int>> tasks = new Queue<IMyTask<int>>();

        private bool works = true;

        public MyThreadPool(int numberOfThreads)
        {
            ThreadCount = numberOfThreads;
            threads = new Thread[numberOfThreads];
            for (int i = 0; i < numberOfThreads; ++i)
            {
                threads[i] = new Thread(() => Console.WriteLine($"Hi from {i} thread"));
            }
        }

        public void Shutdown()
        {
            works = false;
        }

        public bool QueueUserWorkItem(IMyTask<int> callBack)
        {
            if (works)
            {
                tasks.Enqueue(callBack);
                return true;
            }
            throw new Exception();
        }


    }
}
