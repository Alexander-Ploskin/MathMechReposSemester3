using System.Collections.Generic;

namespace MyThreadPoolRealisation
{
    /// <summary>
    /// Implementation of queue, that is thread safety
    /// </summary>
    /// <typeparam name="T">Type of values in the queue</typeparam>
    public class ThreadSafetyQueue<T>
    {
        private Queue<T> queue = new Queue<T>();

        /// <summary>
        /// Adds a new element to the queue, thread enters critical zone during this method
        /// </summary>
        /// <param name="item">New item of the queue</param>
        public void Enqueue(T item)
        {
            lock(queue)
            {
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// Checks if queue is empty, thread enters critical zone during this method
        /// </summary>
        public bool Empty
        {
            get
            {
                lock (queue)
                {
                    return queue.Count == 0;
                }
            }
        }

        /// <summary>
        /// Gets value from the queue and removes this value, thread enters critical zone during this method
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws in case of dequeue from the empty queue</exception>
        /// <returns></returns>
        public T Dequeue()
        {
            lock(queue)
            {
                return queue.Dequeue();
            }
        }

    }
}
