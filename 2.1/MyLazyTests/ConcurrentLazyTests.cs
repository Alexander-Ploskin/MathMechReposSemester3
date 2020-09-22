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
            var lazy = LazyFactory<Thread>.CreateConcurrentLazy(() => Thread.CurrentThread);
            const int numberOfThreads = 10;
            var threads = new Thread[numberOfThreads];
            var results = new Thread[numberOfThreads];
            for (int i = 0; i < numberOfThreads; ++i)
            {
                var localI = i;
                threads[localI] = new Thread(() => results[localI] = lazy.Get());
                threads[localI].Start();
            }
            
            for (int i = 0; i < numberOfThreads; ++i)
            {
                threads[i].Join();
            }
            
            for (int i = 0; i < numberOfThreads; ++i)
            {
                Assert.AreSame(results[0], results[i]);
            }
        }

        [Test]
        public void CalculatingNullFunctionTest()
        {
            Assert.Throws<ArgumentNullException>(() => LazyFactory<object>.CreateSimpleLazy(null));
        }

        [Test]
        public void CalculateFuncThatReturnsNullTest()
        {
            var lazy = LazyFactory<object>.CreateSimpleLazy(() => null);
            var result = lazy.Get();
            Assert.AreEqual(null, result);
        }

    }
}
