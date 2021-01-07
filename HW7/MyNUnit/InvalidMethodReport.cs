using System.Collections.Generic;

namespace MyNUnit
{
    /// <summary>
    /// Report of an invalid method in assembly
    /// </summary>
    public class InvalidMethodReport
    {
        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="name">Name of the method</param>
        /// <param name="error">Error in the method</param>
        public InvalidMethodReport(string name, List<string> error)
        {
            Errors = error;
            Name = name;
        }

        /// <summary>
        /// Name of the method with errors
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// List of errors
        /// </summary>
        public List<string> Errors { get; }
    }
}
