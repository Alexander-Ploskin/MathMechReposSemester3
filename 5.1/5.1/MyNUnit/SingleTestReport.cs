using System;
using System.Collections.Generic;
using System.Text;

namespace MyNUnit
{
    class SingleTestReport
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

        public SingleTestReport(string name, bool passed, TimeSpan time) : this(name, passed, null, null, time) { }

        public string Name { get; }

        public bool Passed { get; }

        public Type Expected { get; }

        public Exception Actual { get; }

        public TimeSpan Time { get; }

        public string IngnoreCause { get; }

        public string FailureMessage { get => "so sad"; }

    }
}
