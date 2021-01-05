using NUnit.Framework;
using MyNUnit;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Reflection;

namespace MyNUnitTests
{
    [TestFixture]
    public class TestAnalyzerTests
    {
        [OneTimeSetUp]
        public void SetUp()
        {
            const string path = "../../../../InvalidTestProject";
            var files = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
            var assemblies = new List<Assembly>();
            foreach (var file in files)
            {
                assemblies.Add(Assembly.LoadFrom(file));
            }
            var classes = assemblies.Distinct().SelectMany(a => a.ExportedTypes).Where(t => t.IsClass);
            report = TestAnalyzer.AnalyzeTestAssembly(classes).Where(r => r.Name == "InvalidTests").FirstOrDefault();
        }

        private InvalidTestClassReport report;

        [TestCase("BeforeClassMethodReturnsSomethingAndHasParams")]
        [TestCase("BeforeMethodReturnsSomethingAndHasParams")]
        [TestCase("TestReturnsSomethingAndHasParams")]
        [TestCase("AfterMethodReturnsSomethingAndHasParams")]
        [TestCase("AfterClassMethodReturnsSomethingAndHasParams")]
        [Test]
        public void MethodReturnsSomethingAndHasParamsTest(string name)
        {
            var methodReport = report.InvalidMethods.Where(r => r.Name == name);
            Assert.AreEqual(1, methodReport.Count());
            Assert.AreEqual(new List<string>() { "should be void", "shouldn't has any parameters" }, methodReport.FirstOrDefault().Errors);
        }

        [TestCase("NotStaticBeforeClassMethod")]
        [TestCase("NotStaticAfterClassMethod")]
        [Test]
        public void StaticNotStaticMethodTest(string name)
        {
            var methodReport = report.InvalidMethods.Where(r => r.Name == name);
            Assert.AreEqual(1, methodReport.Count());
            Assert.AreEqual(new List<string>() { "should be static" }, methodReport.FirstOrDefault().Errors);
        }

        [TestCase("StaticTest")]
        [TestCase("StaticAfterMethod")]
        [TestCase("StaticBeforeMethod")]
        [Test]
        public void NotStaticStaticMethodTest(string name)
        {
            var methodReport = report.InvalidMethods.Where(r => r.Name == name);
            Assert.AreEqual(1, methodReport.Count());
            Assert.AreEqual(new List<string>() { "shouldn't be static" }, methodReport.FirstOrDefault().Errors);
        }

        [TestCase("BeforeClassMethodHasParams")]
        [TestCase("BeforeMethodHasParams")]
        [TestCase("TestHasParams")]
        [TestCase("AfterMethodHasParams")]
        [TestCase("AfterClassMethodHasParams")]
        [Test]
        public void MethodHasParamsTest(string name)
        {
            var methodReport = report.InvalidMethods.Where(r => r.Name == name);
            Assert.AreEqual(1, methodReport.Count());
            Assert.AreEqual(new List<string>() { "shouldn't has any parameters" }, methodReport.FirstOrDefault().Errors);
        }

        [TestCase("BeforeClassMethodReturnsSomething")]
        [TestCase("BeforeMethodReturnsSomething")]
        [TestCase("TestReturnsSomething")]
        [TestCase("AfterMethodReturnsSomething")]
        [TestCase("AfterClassMethodReturnsSomething")]
        [Test]
        public void MethodReturnsSomethingTest(string name)
        {
            var methodReport = report.InvalidMethods.Where(r => r.Name == name);
            Assert.AreEqual(1, methodReport.Count());
            Assert.AreEqual(new List<string>() { "should be void" }, methodReport.FirstOrDefault().Errors);
        }
    }
}
