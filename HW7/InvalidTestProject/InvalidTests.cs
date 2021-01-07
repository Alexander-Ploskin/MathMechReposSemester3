using MyNUnitAttributes;

namespace TestProject
{
    public class InvalidTests
    {
        [BeforeClass]
        public static int BeforeClassMethodReturnsSomethingAndHasParams(int x)
        {
            return x;
        }

        [BeforeClass]
        public static int BeforeClassMethodReturnsSomething()
        {
            return 1;
        }

        [BeforeClass]
        public static void BeforeClassMethodHasParams(int x)
        {
        }

        [BeforeClass]
        public void NotStaticBeforeClassMethod()
        {
        }

        [Before]
        public int BeforeMethodReturnsSomethingAndHasParams(int x)
        {
            return x;
        }

        [Before]
        public int BeforeMethodReturnsSomething()
        {
            return 1;
        }

        [Before]
        public void BeforeMethodHasParams(int x)
        {
        }

        [Before]
        public static void StaticBeforeMethod()
        {
        }

        [Test]
        public int TestReturnsSomething()
        {
            return 1;
        }

        [Test]
        public void TestHasParams(int x)
        {
        }

        [Test]
        public static void StaticTest()
        {
        }

        [Test]
        public int TestReturnsSomethingAndHasParams(int x)
        {
            return x;
        }

        [After]
        public int AfterMethodReturnsSomething()
        {
            return 1;
        }

        [After]
        public void AfterMethodHasParams(int x)
        {
        }

        [After]
        public static void StaticAfterMethod()
        {
        }

        [After]
        public int AfterMethodReturnsSomethingAndHasParams(int x)
        {
            return x;
        }

        [AfterClass]
        public static int AfterClassMethodReturnsSomethingAndHasParams(int x)
        {
            return x;
        }

        [AfterClass]
        public static int AfterClassMethodReturnsSomething()
        {
            return 1;
        }

        [AfterClass]
        public static void AfterClassMethodHasParams(int x)
        {
        }

        [AfterClass]
        public void NotStaticAfterClassMethod()
        {
        }
    }
}
