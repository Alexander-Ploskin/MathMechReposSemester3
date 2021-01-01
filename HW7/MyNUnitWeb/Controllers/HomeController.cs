using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using MyNUnitWeb.Models;
using System.Threading.Tasks;
using System;
using MyNUnit;

namespace MyNUnitWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private CurrentStateModel currentState;

        public HomeController(IWebHostEnvironment environment)
        {
            if (!Directory.Exists($"{environment.WebRootPath}/Temp)"))
            {
                Directory.CreateDirectory($"{environment.WebRootPath}/Temp");
            }
            this.environment = environment;
            currentState = new CurrentStateModel(environment);
        }

        public IActionResult Index()
        {
            return View("Index", currentState);
        }

        public IActionResult History()
        {
            return View("History.chtml");
        }

        public IActionResult Docs()
        {
            return View("Docs.chtml");
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
            currentState.Report.Time = DateTime.Now;
            currentState.Report.ClassReports = reports;
            return RedirectToAction("Index", currentState);
        }
    }
}