using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyNUnitWeb.Models
{
    public class CurrentStateModel
    {
        private readonly IWebHostEnvironment environment;

        public CurrentStateModel(IWebHostEnvironment environment)
            => this.environment = environment;
        
        public IEnumerable<string> Assemblies => Directory.EnumerateFiles($"{environment.WebRootPath}/Temp").
            Select(f => Path.GetFileName(f));

        public List<TestClassReportModel> Reports = new List<TestClassReportModel>();

    }
}