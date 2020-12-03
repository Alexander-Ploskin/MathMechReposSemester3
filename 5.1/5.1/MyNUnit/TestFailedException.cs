using System;

namespace MyNUnit
{
    /// <summary>
    /// Throws if test not passed
    /// </summary>
    public class TestFailedException : Exception
    {
        public TestFailedException() { }

        public TestFailedException(string message) : base(message) { }
    }
}
