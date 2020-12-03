using System;

namespace MyNUnit
{
    public class SingleTestReport
    {
        public SingleTestReport(string name, bool passed, Type expected, Exception actual, TimeSpan time)
        {
            Name = name;
            Passed = passed;
            Expected = expected;
            Actual = actual;
            Time = time;
        }

        public SingleTestReport(string name, string ignoreCause)
            : this(name, false, null, null, TimeSpan.Zero)
        {
            IngnoreCause = ignoreCause;
        }

        public string Name { get; }

        public bool Passed { get; }

        public Type Expected { get; }

        public Exception Actual { get; }

        public TimeSpan Time { get; }

        public string IngnoreCause { get; }

        public bool Ignored { get => IngnoreCause != null; }

        public string Message
        {
            get
            {
                if (Passed)
                {
                    return "";
                }
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
                return "NUnit error";
            }
        }

    }
}
