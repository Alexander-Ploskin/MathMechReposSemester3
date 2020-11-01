using System;

namespace MyThreadPoolRealisation
{
    /// <summary>
    /// Interfaces of tasks in the thread pool
    /// </summary>
    /// <typeparam name="TResult">The type of value, that returns from the function of the task</typeparam>
    public interface IMyTask<out TResult>
    {
        /// <summary>
        /// Is func calculated
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// Result of the function
        /// </summary>
        TResult Result { get; }

        /// <summary>
        /// Creates a new task that gets result from the function of this task
        /// </summary>
        /// <typeparam name="TNewResult">Type of value that produces by the new function</typeparam>
        /// <param name="newFunc">Continuation function</param>
        /// <returns></returns>
        IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newFunc);

    }
}
