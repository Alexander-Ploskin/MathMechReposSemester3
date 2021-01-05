using System;

namespace MyNUnitAttributes
{
    /// <summary>
    /// Attribute of method, that executes as test method
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class TestAttribute : Attribute
    {
        /// <summary>
        /// Expected exception, null if nothing is expected
        /// </summary>
        public Type Expected { get; set; }

        /// <summary>
        /// Reason for ignoring that test
        /// </summary>
        public string Ignore { get; set; }
    }
}
