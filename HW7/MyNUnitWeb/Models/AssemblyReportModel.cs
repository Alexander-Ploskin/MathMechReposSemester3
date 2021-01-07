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

        [Key]
        public string Id { get; set; }
    }
}
