using NUnit.Framework;
using MyNUnit;
using System.Linq;
using System;

namespace MyNUnitTests
{
    public class Tests
    {
        [SetUp]
        public void SetUp()
        {
            reports = TestRunner.RunTests("../../../../TestProject").Where(r => r.ClassName is "Usage").First();
        }

        private TestClassReport reports;

        [Test]
        public void MainInfoTest()
        {
            Assert.AreEqual("TestProject", reports.AssemblyName);
        }

        [Test]
        public void IgnoredMethodTest()
        {
            var report = reports.reports.Where(r => r.Name is "IgnoredTest").First();
            Assert.IsFalse(report.Passed);
            Assert.IsNull(report.Expected);
            Assert.IsNull(report.Actual);
            Assert.AreEqual("allright", report.Message);
            Assert.IsTrue(report.Ignored);
        }

        [Test]
        public void PassedMethodTest()
        {
            var report = reports.reports.Where(r => r.Name is "PassedTest").First();
            Assert.IsTrue(report.Passed);
            Assert.IsNull(report.Expected);
            Assert.IsNull(report.Actual);
            Assert.AreEqual("", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void FailedMethodTest()
        {
            var report = reports.reports.Where(r => r.Name is "FailedTest").First();
            Assert.IsFalse(report.Passed);
            Assert.IsNull(report.Expected);
            Assert.AreEqual(typeof(TestFailedException), report.Actual.GetType());
            Assert.AreEqual("unexpected MyNUnit.TestFailedException", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void PassedTestWithExpectedExceptionTest()
        {
            var report = reports.reports.Where(r => r.Name is "PassedTestWithExpectedException").First();
            Assert.IsTrue(report.Passed);
            Assert.AreEqual(typeof(ArgumentException), report.Expected);
            Assert.AreEqual(typeof(ArgumentException), report.Actual.GetType());
            Assert.AreEqual("expected System.ArgumentException but was System.ArgumentException", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void FailedTestWithExpectedExceptionTest()
        {
            var report = reports.reports.Where(r => r.Name is "FailedTestWithExpectedException").First();
            Assert.IsFalse(report.Passed);
            Assert.AreEqual(typeof(ArgumentException), report.Expected);
            Assert.IsNull(report.Actual);
            Assert.AreEqual("expected System.ArgumentException but was null", report.Message);
            Assert.IsFalse(report.Ignored);
        }

        [Test]
        public void FailedTestWithUnexpectedExceptionTest()
        {
            var report = reports.reports.Where(r => r.Name is "FailedTestWithUnexpectedException").First();
            Assert.IsFalse(report.Passed);
            Assert.AreEqual(typeof(ArgumentException), report.Expected);
            Assert.AreEqual(typeof(ApplicationException), report.Actual.GetType());
            Assert.AreEqual("expected System.ArgumentException but was System.ApplicationException", report.Message);
            Assert.IsFalse(report.Ignored);
        }

    }
}