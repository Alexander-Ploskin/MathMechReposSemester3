using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyNUnitWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using MyNUnit;
using System.Linq;

namespace MyNUnitWeb.Controllers
{
    /// <summary>
    /// Main controller of the application
    /// </summary>
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly CurrentStateModel currentState;
        private readonly TestArchive archive;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="environment">Web host environment</param>
        public HomeController(IWebHostEnvironment environment, TestArchive archive)
        {
            if (!Directory.Exists($"{environment.WebRootPath}/Temp)"))
            {
                Directory.CreateDirectory($"{environment.WebRootPath}/Temp");
            }
            this.environment = environment;
            this.archive = archive;
            currentState = new CurrentStateModel(environment);
        }

        /// <summary>
        /// Loads start page of the application
        /// </summary>
        public IActionResult Index()
        {
            return View("Index", currentState);
        }

        /// <summary>
        /// Uploads new assembly to the temporary directory
        /// </summary>
        /// <param name="file">New assembly</param>
        public IActionResult AddAssembly(IFormFile file)
        {
            if (file != null)
            {
                using (var fileStream = new FileStream($"{environment.WebRootPath}/Temp/{file.FileName}", FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
            }

            return RedirectToAction("Index", currentState);
        }

        /// <summary>
        /// Clears temporary directory
        /// </summary>
        public IActionResult Clear()
        {
            foreach (var file in Directory.GetFiles($"{environment.WebRootPath}/Temp"))
            {
                System.IO.File.Delete(file);
            }
            return RedirectToAction("Index", currentState);
        }

        /// <summary>
        /// Runs tests in all assemblies in the temporary directory
        /// </summary>
        public IActionResult Test()
        {
            try
            {
                var reports = TestRunner.RunTests($"{environment.WebRootPath}/Temp");
                var dateTime = DateTime.Now;
                var results = new List<AssemblyReportModel>();
                foreach (var testClassReport in reports)
                {
                    var assemblyReports = currentState.AssemblyReports.Where(r => r.Name == testClassReport.AssemblyName);
                    var assemblyReport = assemblyReports.FirstOrDefault();
                    if (assemblyReports.Count() == 0)
                    {
                        assemblyReport = new AssemblyReportModel { Name = testClassReport.AssemblyName, Id = dateTime.ToString() + testClassReport.AssemblyName };
                        currentState.AssemblyReports.Add(assemblyReport);
                        results.Add(assemblyReport);
                    }
                    foreach (var test in testClassReport.Reports)
                    {
                        var newTestReportModel = new TestReportModel
                        {
                            ClassName = testClassReport.ClassName,
                            Valid = true,
                            Name = test.Name,
                            Passed = test.Ignored ? null : test.Passed,
                            Time = test.Ignored ? null : test.Time,
                            Message = test.Message,
                            Id = dateTime.ToString() + test.Name + testClassReport.ClassName
                        };

                        assemblyReport.TestReports.Add(newTestReportModel);
                    }
                }
                var newRunReport = new TestRunModel
                {
                    DateTime = DateTime.Now,
                    AssemblyReports = results
                };
                archive.Add(newRunReport);
                archive.SaveChanges();
            }
            catch (InvalidAssemlyException e)
            {
                var reports = e.InvalidClasses;
                foreach (var testClassReport in reports)
                {
                    var assemblyReports = currentState.AssemblyReports.Where(r => r.Name == testClassReport.AssemblyName);
                    var assemblyReport = assemblyReports.FirstOrDefault();
                    if (assemblyReports.Count() == 0)
                    {
                        assemblyReport = new AssemblyReportModel { Name = testClassReport.AssemblyName };
                        currentState.AssemblyReports.Add(assemblyReport);
                    }
                    foreach (var test in testClassReport.InvalidMethods)
                    {
                        var newTestReportModel = new TestReportModel
                        {
                            ClassName = testClassReport.Name,
                            Valid = false,
                            Name = test.Name,
                            Passed = null,
                            Time = null,
                            Message = String.Join("\n", test.Errors)
                        };

                        assemblyReport.TestReports.Add(newTestReportModel);
                    }
                }
            }
           
            return View("Index", currentState);
        }

        public IActionResult History()
        {
            return View("History", archive.RunHistory.Include(run => run.AssemblyReports).ThenInclude(report => report.TestReports).ToList());
        }
    }
}
