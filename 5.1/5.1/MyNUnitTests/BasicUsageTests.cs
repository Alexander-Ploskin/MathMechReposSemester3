using NUnit.Framework;
using System.Linq;
using System;
using MyNUnit;

namespace MyNUnitTests
{
    public class Tests
    {
        [SetUp]
        public void SetUp()
        {
            var reports = TestRunner.RunTests("../../../../TestProject");
            basicUsageReports = reports.Where(r => r.ClassName == "Usage").FirstOrDefault();
            beforeAfterMethodsReports = reports.Where(r => r.ClassName == "BeforeAfterMethods").FirstOrDefault();
        }

        private TestClassReport basicUsageReports;
        private TestClassReport beforeAfterMethodsReports;

        [Test]
        public void MainInfoTest()
        {
            Assert.AreEqual("TestProject", basicUsageReports.AssemblyName);
        }

        [Test]
        public void IgnoredMethodTest()
        {
            var report = basicUsageReports.reports.Where(r => r.Name is "IgnoredTest").First();
            Assert.IsFalse(report.Passed);
            Assert.IsNull(report.Expected);
            Assert.IsNull(report.Actual);
            Assert.AreEqual("allright", report.Message);
            Assert.IsTrue(report.Ignored);
        }

        [Test]
        public void PassedMethodTest()
        {
            var report = basicUsageReports.reports.Where(r => r.Name is "PassedTest").First();
            Assert.IsTrue(report.Passed);
            Assert.IsNull(report.Expected);
            Assert.IsNull(report.Actual);
            Assert.AreEqual("", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void FailedMethodTest()
        {
            var report = basicUsageReports.reports.Where(r => r.Name is "FailedTest").First();
            Assert.IsFalse(report.Passed);
            Assert.IsNull(report.Expected);
            Assert.AreEqual(typeof(ApplicationException), report.Actual.GetType());
            Assert.AreEqual("unexpected System.ApplicationException", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void PassedTestWithExpectedExceptionTest()
        {
            var report = basicUsageReports.reports.Where(r => r.Name is "PassedTestWithExpectedException").First();
            Assert.IsTrue(report.Passed);
            Assert.AreEqual(typeof(ArgumentException), report.Expected);
            Assert.AreEqual(typeof(ArgumentException), report.Actual.GetType());
            Assert.AreEqual("expected System.ArgumentException but was System.ArgumentException", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void FailedTestWithExpectedExceptionTest()
        {
            var report = basicUsageReports.reports.Where(r => r.Name is "FailedTestWithExpectedException").First();
            Assert.IsFalse(report.Passed);
            Assert.AreEqual(typeof(ArgumentException), report.Expected);
            Assert.IsNull(report.Actual);
            Assert.AreEqual("expected System.ArgumentException but was null", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void FailedTestWithUnexpectedExceptionTest()
        {
            var report = basicUsageReports.reports.Where(r => r.Name is "FailedTestWithUnexpectedException").First();
            Assert.IsFalse(report.Passed);
            Assert.AreEqual(typeof(ArgumentException), report.Expected);
            Assert.AreEqual(typeof(ApplicationException), report.Actual.GetType());
            Assert.AreEqual("expected System.ArgumentException but was System.ApplicationException", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void BeforeAfterMethodsTest()
        {
            Assert.IsTrue(beforeAfterMethodsReports.reports.Any(r => r.Passed));
        }
    }
}