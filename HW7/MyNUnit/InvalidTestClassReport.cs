using System.Collections.Generic;

namespace MyNUnit
{
    /// <summary>
    /// Report of invalid test class
    /// </summary>
    public class InvalidTestClassReport
    {
        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="name">Name of the class</param>
        /// <param name="invalidMethods">Collection of invalid methods reports</param>
        public InvalidTestClassReport(string name, ICollection<InvalidMethodReport> invalidMethods, string assemblyName)
        {
            Name = name;
            AssemblyName = assemblyName;
            InvalidMethods = invalidMethods;
        }

        /// <summary>
        /// Name of assembly that contains class
        /// </summary>
        public string AssemblyName { get; }

        /// <summary>
        /// Name of the class
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Reports of invalid methods
        /// </summary>
        public ICollection<InvalidMethodReport> InvalidMethods { get; }
    }
}
