using System;
using System.Linq;
using NUnit.Framework;
using MyNUnit;

namespace MyNUnitTests
{
    class BeforeAfterMethodsTest
    {
        [SetUp]
        public void SetUp()
        {
            reports = TestRunner.RunTests("../../../../TestProject").Where(r => r.ClassName is "BeforeAfterTests").First();
        }

        [Test]
        public void BeforeMethodsTest()
        {
            var report = reports.reports.Where(r => r.Name is "CheckBefore").First();
            Assert.IsTrue(report.Passed);
        }

        [Test]
        public void AfterMethodsTest()
        {
            var report = reports.reports.Where(r => r.Name is "CheckAfter").First();
            Assert.IsTrue(report.Passed);
        }

        private TestClassReport reports;
    }
}
