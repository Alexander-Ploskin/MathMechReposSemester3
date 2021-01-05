using System;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// Model responding a correct test
    /// </summary>
    public class TestReportModel
    {
        /// <summary>
        /// Name of a test
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// If test ignored or not
        /// </summary>
        public bool Ignored { get; set; }
        
        /// <summary>
        /// If test passed ot not, null if test was ignored
        /// </summary>
        public bool Passed { get; set; }
        
        /// <summary>
        /// Time of a test execution
        /// </summary>
        public TimeSpan Time { get; set; }
        
        /// <summary>
        /// Reason of failure or ignore of a test
        /// </summary>
        public string Message { get; set; }
    }
}