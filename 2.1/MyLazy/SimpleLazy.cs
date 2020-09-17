using System;

namespace MyLazy
{
    /// <summary>
    /// Implementation of ILazy for one thread programs
    /// </summary>
    /// <typeparam name="T">Type of the return value of the function</typeparam>
    public class SimpleLazy<T> : ILazy<T>
    {
        /// <summary>
        /// Returns the new lazy object
        /// </summary>
        /// <param name="supplier">function to calculating</param>
        public SimpleLazy(Func<T> supplier)
        {
            if (supplier == null)
            {
                throw new ArgumentNullException();
            }
            this.supplier = supplier;
        }

        private readonly Func<T> supplier;

        private T result = default(T);

        private bool calculated = false;

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
            result = supplier();
            calculated = true;
            return result;
        }

    }
}
