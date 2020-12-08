using MyNUnitAttributes;

namespace TestProject
{
    public class InvalidBeforeAfter
    {
        [Before]
        public int BeforeReturnsSomething()
        {
            return 1;
        }

        [Test]
        public void Test()
        {
        }

        [After]
        public int AfterReturnsSomething()
        {
            return 1;
        }

    }
}
