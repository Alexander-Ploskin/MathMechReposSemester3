using System;

namespace MyNUnit.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class TestAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the type of expected exception.
        /// </summary>
        public Type Expected { get; set; }

        /// <summary>
        /// Reason for ignore the test.
        /// </summary>
        public string Ignore { get; set; }
    }
}
