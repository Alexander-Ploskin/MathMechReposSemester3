using System;

namespace MyLazy
{
    /// <summary>
    /// Implementation of ILazy for multithreading programs 
    /// </summary>
    /// <typeparam name="T">Type of a return value of the function</typeparam>
    class ConcurrentLazy<T> : ILazy<T>
    {
        public ConcurrentLazy(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException(nameof(supplier));
            }
            this.supplier = supplier;
        }

        private readonly Object lockObject = new Object();

        private  Func<T> supplier;

        private T result = default(T);

        private volatile bool calculated = false;

        /// <summary>
        /// Gives a result of the supplier function 
        /// </summary>
        /// <returns>Result of the calculation</returns>
        public T Get()
        {
            if (calculated)
            {
                return result;
            }

            lock (lockObject)
            {
                if (calculated)
                {
                    return result;
                }

                result = supplier();
                supplier = null;
                calculated = true;
                return result;
            }
        }

    }
}
