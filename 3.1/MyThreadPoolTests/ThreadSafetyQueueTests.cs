using NUnit.Framework;

namespace MyThreadPoolTests
{
    using MyThreadPoolRealisation;
    using System;
    using System.Threading;

    class ThreadSafetyQueueTests
    {
        private ThreadSafetyQueue<int> queue;

        [SetUp]
        public void SetUp()
        {
            queue = new ThreadSafetyQueue<int>();
        }

        [Test]
        public void EnqueueTest()
        {
            const int threadsCount = 2;
            var threads = new Thread[threadsCount];
            var countdownEvent = new CountdownEvent(threadsCount);
            for (int i = 0; i < threadsCount; ++i)
            {
                threads[i] = new Thread(() =>
                {
                    queue.Enqueue(1);
                    countdownEvent.Signal();
                });
                threads[i].Start();
            }
            countdownEvent.Wait();
            Assert.AreEqual(1, queue.Dequeue());
            Assert.AreEqual(1, queue.Dequeue());
        }

        [Test]
        public void EmptyTest()
        {
            const int threadsCount = 2;
            var threads = new Thread[threadsCount];
            using var countDownEvent1 = new CountdownEvent(1);
            using var countDownEvent2 = new CountdownEvent(1);
            using var countDownEvent3 = new CountdownEvent(1);
            threads[0] = new Thread(() =>
            {
                queue.Enqueue(1);
                countDownEvent1.Signal();
                countDownEvent2.Wait();
                queue.Dequeue();
                countDownEvent3.Signal();
            });
            threads[0].Start();

            threads[1] = new Thread(() =>
            {
                countDownEvent1.Wait();
                Assert.IsFalse(queue.Empty);
                countDownEvent2.Signal();
                countDownEvent3.Wait();
                Assert.IsTrue(queue.Empty);
            });
            threads[1].Start();
        }

        [Test]
        public void DequeueTest()
        {
            const int threadsCount = 2;
            var threads = new Thread[threadsCount];
            using var countDownEvent = new CountdownEvent(1);
            queue.Enqueue(1);
            threads[0] = new Thread(() =>
            {
                queue.Dequeue();
                countDownEvent.Signal();
            });
            threads[0].Start();
            threads[1] = new Thread(() =>
            {
                countDownEvent.Wait();
                Assert.Throws<InvalidOperationException>(() => queue.Dequeue());
            });
            threads[1].Start();
        }

    }
}
