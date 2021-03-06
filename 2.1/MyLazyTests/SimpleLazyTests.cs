using NUnit.Framework;

namespace MyLazyTests
{
    using MyLazy;
    using System;
    using System.Threading;

    public class Tests
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