using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyNUnitWeb.Models
{
    public class TestRunModel
    {
        /// <summary>
        /// Date and time of test execution
        /// </summary>
        [Key]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Collection of reports of tested assemblies
        /// </summary>
        public List<AssemblyReportModel> AssemblyReports { get; set; } = new List<AssemblyReportModel>();
    }
}
