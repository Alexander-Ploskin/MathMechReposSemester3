using NUnit.Framework;

namespace MyThreadPoolTests
{
    using MyThreadPoolRealisation;
    using System;
    using System.Threading;

    class MyThreadPoolTests
    {
        private MyThreadPool threadPool;
        private readonly int threadsCount = 8;

        [SetUp]
        public void SetUp()
        {
            threadPool = new MyThreadPool(threadsCount);
        }

        [Test]
        public void InvalidNumbersOfThreadsTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => threadPool = new MyThreadPool(-1));
            Assert.Throws<ArgumentOutOfRangeException>(() => threadPool = new MyThreadPool(0));
        }

        [Test]
        public void ThreadCountTest()
        {
            using var countDownEventForTestThread = new CountdownEvent(threadsCount);
            using var countDownEventForTasks = new CountdownEvent(1);
            int callsCount = 0;
            for (var i = 0; i < threadsCount * 2; i++)
            {
                threadPool.Submit(() =>
                {
                    Interlocked.Increment(ref callsCount);
                    countDownEventForTestThread.Signal();
                    countDownEventForTasks.Wait();
                    return true;
                });
            }

            countDownEventForTestThread.Wait();
            Assert.AreEqual(threadsCount, callsCount);
        }

        [Test]
        public void SubmitNullTest() => Assert.Throws<ArgumentNullException>(() => threadPool.Submit<int>(null));

        [Test]
        public void CalculationTest()
        {
            var task = threadPool.Submit(() => 2 + 2 * 2);
            Assert.AreEqual(6, task.Result);
            Assert.IsTrue(task.IsCompleted);
        }

        [Test]
        public void NotCompletedBeforeHaveResultTest()
        {
            var mutex = new Mutex();
            var task = threadPool.Submit(() =>
            {
                mutex.WaitOne();
                return 5;
            });
            Assert.IsFalse(task.IsCompleted);
        }

        [Test]
        public void SimpleContinueWithTest() => Assert.AreEqual("4", threadPool.Submit(() => 2 * 2).ContinueWith(x => x.ToString()).Result);

        [Test]
        public void DoubleContinueWithTest() => Assert.AreEqual(3, threadPool.Submit(() => 20 * 20).ContinueWith(x => x.ToString()).ContinueWith(x => x.Length).Result);

        [Test]
        public void ContinueWithNullTest() => Assert.Throws<ArgumentNullException>(() => threadPool.Submit(() => 2 * 2).ContinueWith<int>(null));

        [Test]
        public void MultithreadingContinueWithTest()
        {
            var numberOfThreads = Environment.ProcessorCount;
            var threads = new Thread[numberOfThreads];
            var results = new string[numberOfThreads];
            using var countdownEvent = new CountdownEvent(1);

            var numberOfCalls = 0;
            var task = threadPool.Submit(() =>
            {
                countdownEvent.Wait();
                return 1;
            });
            var continueWithTask = task.ContinueWith((x) =>
            {
                Interlocked.Increment(ref numberOfCalls);
                return x.ToString();
            });

            for (var i = 0; i < threads.Length; i++)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    results[localI] = continueWithTask.Result;
                });
                threads[i].Start();
            }

            countdownEvent.Signal();
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual(1, numberOfCalls);
            foreach (var result in results)
            {
                Assert.AreEqual("1", result);
            }
        }

        [Test]
        public void ShutdownedThreadPoolTakeNewTasksTest()
        {
            for (int i = 0; i < 100; ++i)
            {
                threadPool.Submit(() => 2);
            }
            threadPool.Shutdown();
            Assert.Throws<ApplicationException>(() => threadPool.Submit(() => 2));
        }

        [Test]
        public void ShutdownedThreadPoolExecutesAlreadyTakenTasksTest()
        {
            const int numberOfTasks = 10;
            var tasks = new IMyTask<int>[numberOfTasks];
            for (int i = 0; i < numberOfTasks; ++i)
            {
                var localI = i;
                tasks[i] = threadPool.Submit(() => localI * localI);
            }
            threadPool.Shutdown();
            for (int i = 0; i < numberOfTasks; ++i)
            {
                Assert.AreEqual(i * i, tasks[i].Result);
            }
        }

        [Test]
        public void FuncsThrowsExceptionsTest()
        {
            const string message = "dangerous exception";
            var task = threadPool.Submit<int>(() => throw new Exception(message));
            try
            {
                var result = task.Result;
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(AggregateException), e.GetType());
                Assert.AreEqual(message, e.InnerException.Message);
            }
        }

        [Test]
        public void ContinueWithFuncThatThrowsException()
        {
            const string message = "dangerous exception";
            var task = threadPool.Submit(() => 2).ContinueWith<int>((x) => throw new Exception(message));
            try
            {
                var result = task.Result;
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(AggregateException), e.GetType());
                Assert.AreEqual(message, e.InnerException.Message);
            }
        }

        [Test]
        public void ShutdownEmptyThreadPoolTest()
        {
            threadPool.Shutdown();
            Assert.Pass();
        }

        [Test]
        public void WaitingTasksTest()
        {
            const int numberOfTasks = 10;
            var tasks = new IMyTask<int>[numberOfTasks];
            for (int i = 0; i < numberOfTasks; ++i)
            {
                var localI = i;
                tasks[i] = threadPool.Submit(() => localI);
            }

            for (int i = 0; i < numberOfTasks; ++i)
            {
                Assert.AreEqual(i, tasks[i].Result);
            }
        }

        [Test]
        public void ContinueWithInShutdownedThreadPoolTest()
        {
            var task = threadPool.Submit(() => true);
            threadPool.Shutdown();
            Assert.Throws<ApplicationException>(() => task.ContinueWith(x => true));
        }

    }
}
