using System;

namespace MyNUnit
{
    /// <summary>
    /// Provides info about execution of one test method
    /// </summary>
    public class SingleTestReport
    {
        /// <summary>
        /// Basic constructor for executed tests
        /// </summary>
        /// <param name="name">Test method name</param>
        /// <param name="passed">True if test passed</param>
        /// <param name="expected">Type of expected exception, null if nothing is expected</param>
        /// <param name="actual">Thrown exception, null if nothing was thrown</param>
        /// <param name="time">Time of test execution</param>
        public SingleTestReport(string name, Type expected, Exception actual, TimeSpan time)
        {
            Name = name;
            Expected = expected;
            Actual = actual;
            Time = time;
        }

        public SingleTestReport(string name, string ignoreCause)
            : this(name, null, null, TimeSpan.Zero)
        {
            IngnoreCause = ignoreCause;
        }

        public string Name { get; }

        public bool Passed
        {
            get
            {
                if (Expected != null && Actual != null)
                {
                    return Expected == Actual.GetType();
                }
                if (Expected == null && Actual == null)
                {
                    return true;
                }
                return false;
            }
        }

        public Type Expected { get; }

        public Exception Actual { get; }

        public TimeSpan Time { get; }

        public string IngnoreCause { get; }

        public bool Ignored { get => IngnoreCause != null; }

        public string Message
        {
            get
            {
                if (Ignored)
                {
                    return IngnoreCause;
                }
                if (Expected != null && Actual == null)
                {
                    return $"expected {Expected} but was null";
                }
                else if (Expected == null && Actual != null)
                {
                    return $"unexpected {Actual.GetType()}";
                }
                else if (Expected != null && Actual != null)
                {
                    return $"expected {Expected} but was {Actual.GetType()}";
                }
                return "";
            }
        }

    }
}
