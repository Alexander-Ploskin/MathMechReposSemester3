using NUnit.Framework;

namespace MyLazyTests
{
    using MyLazy;
    using System;
    using System.Threading;

    class ConcurrentLazyTests
    {
        [Test]
        public void CorrectValueTest()
        {
            var lazy = LazyFactory<int>.CreateSimpleLazy(() => 345);
            Assert.AreEqual(345, lazy.Get());
            Assert.AreEqual(345, lazy.Get());
        }

        [Test]
        public void CalculatingOnceTest()
        {
            var lazy = LazyFactory<DateTime>.CreateSimpleLazy(() => DateTime.Now);
            var firstResult = lazy.Get();
            Thread.Sleep(100);
            var secondResult = lazy.Get();
            Assert.AreEqual(firstResult, secondResult);
        }

        [Test]
        public void ManyThreadsTest()
        {
            var lazy = LazyFactory<DateTime>.CreateConcurrentLazy(() => DateTime.Now);
            var firstResult = DateTime.Now;
            var secondResult = DateTime.Now;
            var firstThread = new Thread(() => firstResult = lazy.Get());
            var secondThread = new Thread(() => secondResult = lazy.Get());
            firstThread.Start();
            secondThread.Start();
            firstThread.Join();
            secondThread.Join();
            Assert.AreEqual(firstResult, secondResult);
        }
    }
}
