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
        public ConcurrentBag<SingleTestReport> Reports { get; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        public TestClassReport(string assemblyName, string className)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            Reports = new ConcurrentBag<SingleTestReport>();
        }
    }
}
