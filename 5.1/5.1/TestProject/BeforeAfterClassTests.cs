using MyNUnit;
using MyNUnit.Attributes;

namespace TestProject
{
    public class BeforeAfterClassTests
    {
        private static bool[] checklist = new bool[4] { false, false, false, false };

        [BeforeClass]
        public static void BeforeClass1()
        {
            checklist[0] = true;
        }

        [BeforeClass]
        public static void BeforeClass2()
        {
            checklist[1] = true;
        }

        [Test]
        public void Check1()
        {
            if (!checklist[0] || !checklist[1] || checklist[2] || checklist[3])
            {
                throw new TestFailedException();
            }
        }

        [Test]
        public void Check2()
        {
            if (!checklist[0] || !checklist[1] || checklist[2] || checklist[3])
            {
                throw new TestFailedException();
            }
        }

        [AfterClass]
        public static void AfterClass1()
        {
            checklist[2] = true;
        }

        [AfterClass]
        public static void AfterClass2()
        {
            checklist[3] = true;
            if (!checklist[0] || !checklist[1] || !checklist[2] || !checklist[3])
            {
                throw new TestFailedException();
            }
        }
    }
}
