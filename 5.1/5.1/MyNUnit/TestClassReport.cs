using System.Collections.Concurrent;

namespace MyNUnit
{
    /// <summary>
    /// Provides info about executed test class
    /// </summary>
    public class TestClassReport
    {
        /// <summary>
        /// Name of assembly that contains class
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// Name of the class
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// Report for every test, that belongs to the class
        /// </summary>
        public readonly ConcurrentBag<SingleTestReport> reports;

        /// <summary>
        /// Basic constructor
        /// </summary>
        public TestClassReport(string assemblyName, string className)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            reports = new ConcurrentBag<SingleTestReport>();
        }

    }
}
