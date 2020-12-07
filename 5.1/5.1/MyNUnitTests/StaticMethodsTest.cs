using NUnit.Framework;
using MyNUnit;
using System.Linq;

namespace MyNUnitTests
{
    class StaticMethodsTests
    {
        [Test]
        public void StaticMethodsTest()
        {
            var reports = TestRunner.RunTests("../../../../TestProject").Where(r => r.ClassName == "StaticMethods").FirstOrDefault();
            Assert.IsTrue(reports.reports.Any(r => r.Passed));
        }
    }
}
