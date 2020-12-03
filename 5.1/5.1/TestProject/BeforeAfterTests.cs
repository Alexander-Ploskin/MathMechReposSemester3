using System;
using MyNUnit.Attributes;

namespace TestProject
{
    class BeforeAfterTests
    {
        private bool[] checklist = new bool[4]{ false, false, false, false };

        [Before]
        public void Before1()
        {
            checklist[0] = true;
        }

        [Before]
        public void Before2()
        {
            checklist[1] = true;
        }

        [After]
        public void After1()
        {
            checklist[2] = true;
        }

        [After]
        public void After2()
        {
            checklist[3] = true;
        }

        [Test]
        public void Check()
        {
            foreach (var item in checklist)
            {
                if (!item)
                {
                    throw new ApplicationException();
                }
            }
        }

    }
}
