using System;

namespace MyThreadPool
{
    class AggregateException : ApplicationException
    {
        public Exception exception { get; }
        public AggregateException(Exception e)
        {
            exception = e;
        }

    }
}
