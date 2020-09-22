using System;

namespace MyLazy
{
    /// <summary>
    /// Lazies creator
    /// </summary>
    /// <typeparam name="T">Type of the return value in the func of lazy</typeparam>
    public static class LazyFactory<T>
    {
        /// <summary>
        /// Creates the simple lazy
        /// </summary>
        /// <param name="supplier">Function in the new lazy</param>
        /// <returns>New lazy object</returns>
        public static ILazy<T> CreateSimpleLazy(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }
            return new SimpleLazy<T>(supplier);
        }

        /// <summary>
        /// Creates the multithreading safe lazy
        /// </summary>
        /// <param name="supplier">Function in the new lazy</param>
        /// <returns>New lazy object</returns>
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
