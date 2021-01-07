using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MyNUnitWeb.Models
{
    /// <summary>
    /// Provides current state of the application
    /// </summary>
    public class CurrentStateModel
    {
        private readonly IWebHostEnvironment environment;

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="environment">Web host environment</param>
        public CurrentStateModel(IWebHostEnvironment environment)
            => this.environment = environment;

        /// <summary>
        /// Current uploaded assemblies
        /// </summary>
        public IEnumerable<string> Assemblies => Directory.EnumerateFiles($"{environment.WebRootPath}/Temp").
            Select(f => Path.GetFileName(f));

        /// <summary>
        /// Reports to tested assemblies
        /// </summary>
        public List<AssemblyReportModel> AssemblyReports = new List<AssemblyReportModel>();
    }
}
