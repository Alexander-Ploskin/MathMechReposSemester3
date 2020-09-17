using System;

namespace MyLazy
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class LazyFactory<T>
    {

        public static ILazy<T> CreateSimpleLazy(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }
            return new SimpleLazy<T>(supplier);
        }

        public static ILazy<T> CreateConcurrentLazy(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }

            return new ConcurrentLazy<T>(supplier);
        }

    }
}
