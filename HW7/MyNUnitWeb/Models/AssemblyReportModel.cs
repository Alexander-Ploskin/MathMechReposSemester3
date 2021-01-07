using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// Represents report to test assembly
    /// </summary>
    public class AssemblyReportModel
    {
        /// <summary>
        /// Name of the assembly
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Collection of reports to test methods
        /// </summary>
        public List<TestReportModel> TestReports { get; set; } = new List<TestReportModel>();

        /// <summary>
        /// True if assembly is valid else false
        /// </summary>
        public bool Valid { get => !TestReports.Any(r => !r.Valid); }

        /// <summary>
        /// Unique value to be key in db
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// Number of passed tests
        /// </summary>
        public int Passed { get; set; }

        /// <summary>
        /// Number of failed tests
        /// </summary>
        public int Failed { get; set; }

        /// <summary>
        /// Number of ignored tests
        /// </summary>
        public int Ignored { get; set; }
    }
}
