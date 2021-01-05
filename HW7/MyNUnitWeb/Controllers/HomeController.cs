using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyNUnitWeb.Models;
using MyNUnit;

namespace MyNUnitWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly TestArchive archive;
        private CurrentStateModel currentState;

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

        public IActionResult Index()
        {
            return View("Index", currentState);
        }

        public IActionResult History()
        {
            return View("History", archive.TestRuns.Include("Reports").ToList());
        }

        public IActionResult Docs()
        {
            return View("Docs");
        }

        [HttpPost]
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
    
        public IActionResult Test()
        {
            var reports = TestRunner.RunTests($"{environment.WebRootPath}/Temp");
            var testRunReport = new TestRunModel { DateTime = DateTime.Now };
            foreach (var classReport in reports)
            {
                var classReportModel = new TestClassReportModel
                {
                    AssemblyName = classReport.AssemblyName,
                    Name = classReport.ClassName
                };
                foreach (var testReport in classReport.Reports)
                {
                    var testReportModel = new TestReportModel
                    {
                        Name = testReport.Name,
                        Ignored = testReport.Ignored,
                        Message = testReport.Message,
                        Passed = testReport.Passed,
                        Time = testReport.Time
                    };
                    classReportModel.TestReports.Add(testReportModel);
                }
                
                currentState.Reports.Add(classReportModel);
                testRunReport.Reports.Add(classReportModel);
            }

            archive.TestRuns.Add(testRunReport);
            archive.SaveChanges();
            return View("Index", currentState);
        }

        public IActionResult Clear()
        {
            foreach (var file in Directory.GetFiles($"{environment.WebRootPath}/Temp"))
            {
                System.IO.File.Delete(file);
            }
            return RedirectToAction("Index", currentState);
        }
    }
}