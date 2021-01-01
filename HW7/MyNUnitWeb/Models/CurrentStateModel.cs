using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyNUnitWeb.Models
{
    public class CurrentStateModel
    {
        private readonly IWebHostEnvironment environment;

        public CurrentStateModel(IWebHostEnvironment environment)
        {
            this.environment = environment;
            Report = new ReportModel();
        }

        public IEnumerable<string> Assemblies => Directory.EnumerateFiles($"{environment.WebRootPath}/Temp").
            Select(f => Path.GetFileName(f));

        public ReportModel Report { get; private set; }

    }
}