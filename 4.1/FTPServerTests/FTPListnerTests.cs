using NUnit.Framework;
using System.IO;

namespace FTPServerTests
{
    public class FTPListnerTests
    {
        private MemoryStream stream;
        private StreamReader reader;

        [SetUp]
        public void Setup()
        {
            stream = new MemoryStream();
            reader = new StreamReader(stream);

        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}