using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyNUnitWeb.Models
{
    public class TestRunModel
    {
        [Key]
        public DateTime DateTime { get; set; }

        public readonly List<TestClassReportModel> Reports = new List<TestClassReportModel>();
    }
}