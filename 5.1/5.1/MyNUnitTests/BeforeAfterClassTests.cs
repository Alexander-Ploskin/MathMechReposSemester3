using System.Linq;
using MyNUnit;
using NUnit.Framework;

namespace MyNUnitTests
{
    class BeforeAfterClassTests
    {
        [Test]
        public void StaticMethodsTest()
        {
            try
            {
                var reports = TestRunner.RunTests("../../../../TestProject").Where(r => r.ClassName is "BeforeAfterClassTests").First();
                var report1 = reports.reports.Where(r => r.Name is "Check1").First();
                var report2 = reports.reports.Where(r => r.Name is "Check2").First();
                Assert.IsTrue(report1.Passed && report2.Passed);
            }
            catch (TestFailedException)
            {
                Assert.Fail();
            }
        }

    }
}
