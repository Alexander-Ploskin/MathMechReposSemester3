using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyNUnit;

namespace MyNUnitWeb.Models
{
    public class ReportModel
    {
        public IEnumerable<TestClassReport> ClassReports { get; set; }

        public DateTime Time { get; set; }

    }
}