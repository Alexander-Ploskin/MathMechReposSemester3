using System.Collections.Generic;

namespace MyNUnitWeb.Models
{
    public class TestClassReportModel
    {
        public string AssemblyName { get; set; }
        
        public string Name { get; set; }
        
        public List<TestReportModel> TestReports = new List<TestReportModel>();
    }
}