using MyNUnit.Attributes;
using MyNUnit;
using System.Threading;

namespace TestProject
{
    public class BeforeAfterMethods
    {
        private bool[] checklist = new bool[4] { false, false, false, false };


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

        [Test]
        public void Test()
        {
            if (!checklist[0] || !checklist[1] || checklist[2] || checklist[3])
            {
                throw new TestFailedException();
            }
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

        [After]
        public void After3()
        {
            if (!checklist[0] || !checklist[1] || !checklist[2] || !checklist[3])
            {
                throw new TestFailedException();
            }
        }
    }
}
