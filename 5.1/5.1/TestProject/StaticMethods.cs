using MyNUnitAttributes;
using System;

namespace TestProject
{
    public class StaticMethods
    {
        private static bool[] checklist = new bool[4] { false, false, false, false };


        [BeforeClass]
        public static void Before1()
        {
            checklist[0] = true;
        }

        [BeforeClass]
        public static void Before2()
        {
            checklist[1] = true;
        }

        [Test]
        public void Test()
        {
            if (!checklist[0] || !checklist[1] || checklist[2] || checklist[3])
            {
                throw new ApplicationException();
            }
        }

        [AfterClass]
        public static void After1()
        {
            checklist[2] = true;
        }

        [AfterClass]
        public static void After2()
        {
            checklist[3] = true;
        }

        [AfterClass]
        public static void After3()
        {
            if (!checklist[0] || !checklist[1] || !checklist[2] || !checklist[3])
            {
                throw new ApplicationException();
            }
        }
    }
}
