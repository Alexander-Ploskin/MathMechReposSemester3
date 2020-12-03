using System;
using MyNUnit.Attributes;
using MyNUnit;

namespace TestProject
{
    public class Usage
    {
        [Test(Ignore = "allright")]
        public void IgnoredTest()
        {
            throw new ApplicationException("This method should be ignored");
        }

        [Test]
        public void PassedTest()
        {
        }

        [Test]
        public void FailedTest()
        {
            throw new TestFailedException();
        }

        [Test(Expected = typeof(ArgumentException))]
        public void PassedTestWithExpectedException()
        {
            throw new ArgumentException();
        }

        [Test(Expected = typeof(ArgumentException))]
        public void FailedTestWithExpectedException()
        {
        }

        [Test(Expected = typeof(ArgumentException))]
        public void FailedTestWithUnexpectedException()
        {
            throw new ApplicationException();
        }

    }
}
