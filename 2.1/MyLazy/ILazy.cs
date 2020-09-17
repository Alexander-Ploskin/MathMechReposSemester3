namespace MyLazy
{
    /// <summary>
    /// Interface of lazy objects
    /// </summary>
    /// <typeparam name="T">Type of the return value of the fuction</typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// Calculates calculating once, then gives result of the calculation
        /// </summary>
        /// <returns>Result of evaluation</returns>
        T Get();
    }
}
