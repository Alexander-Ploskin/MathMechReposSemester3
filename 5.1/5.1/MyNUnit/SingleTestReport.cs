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

        /// <summary>
        /// Constructor for ignored tests
        /// </summary>
        /// <param name="name">Name of the test</param>
        /// <param name="ignoreCause">Cause of the ignore</param>
        public SingleTestReport(string name, string ignoreCause)
            : this(name, null, null, TimeSpan.Zero)
        {
            IngnoreCause = ignoreCause;
        }

        /// <summary>
        /// Name of the test method
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// True if test was passed else false
        /// </summary>
        public bool Passed
        {
            get
            {
                if (Ignored)
                {
                    return false;
                }
                if (Expected != null && Actual != null)
                {
                    return Expected == Actual.GetType();
                }
                return Expected == null && Actual == null;
            }
        }

        /// <summary>
        /// Type of expected exception
        /// </summary>
        public Type Expected { get; }

        /// <summary>
        /// Acually thrown exception
        /// </summary>
        public Exception Actual { get; }

        /// <summary>
        /// Time of test execution
        /// </summary>
        public TimeSpan Time { get; }

        /// <summary>
        /// Cause of ignore
        /// </summary>
        private string IngnoreCause { get; }

        /// <summary>
        /// True if test wasn't executed
        /// </summary>
        public bool Ignored { get => IngnoreCause != null; }

        /// <summary>
        /// Message of the test
        /// </summary>
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
