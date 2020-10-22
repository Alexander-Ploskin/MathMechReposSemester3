using NUnit.Framework;

namespace FTPServerTests
{
    using FTPServer;
    using System.Text;

    public class Tests
    {
        [Test]
        [TestCase("")]
        [TestCase("3 path")]
        [TestCase(" ")]
        [TestCase("1path")]
        [TestCase("2path")]
        [TestCase("jorgneowgjogirwo[g")]
        [TestCase("")]
        public void InvalidRequestsTest(string request)
        {
            var response = FTPRequestsHandler.HadleRequest(request);
            Assert.AreEqual(FTPRequestsHandler.ErrorMessage, response.message.Substring(0, 2));
            Assert.AreEqual(null, response.stream);
        }

        [Test]
        [TestCase("1 ./folder/folder")]
        [TestCase("1 ./folder/folder")]
        public void InvalidPathRequestsTest(string request)
        {
            var response = FTPRequestsHandler.HadleRequest(request);
            Assert.AreEqual(FTPRequestsHandler.ErrorMessage, response.message.Substring(0, 2));
            Assert.AreEqual(null, response.stream);
        }

        [Test]
        public void EmptyDirectoryListTest()
        {
            const string path = "../../../../TestData/EmptyFolder";
            const string expectedResponse = "0";
            var response = FTPRequestsHandler.HadleRequest("1 " + path);
            Assert.AreEqual(expectedResponse, response.message);
            Assert.AreEqual(null, response.stream);
        }

        [Test]
        public void NotEmptyDirectoryTest()
        {
            const string path = "../../../../TestData/NotEmptyFolder";
            const string expectedResponse = "3 ..\\..\\..\\..\\TestData\\NotEmptyFolder\\File1.txt false  " +
                "..\\..\\..\\..\\TestData\\NotEmptyFolder\\Dir1 true ..\\..\\..\\..\\TestData\\NotEmptyFolder\\Dir2 true";
            var response = FTPRequestsHandler.HadleRequest("1 " + path);
            Assert.AreEqual(expectedResponse, response.message);
            Assert.AreEqual(null, response.stream);
        }

        [Test]
        public void GetTest()
        {
            const string path = "../../../../TestData/NotEmptyFolder/File1.txt";
            var response = FTPRequestsHandler.HadleRequest("2 " + path);
            const string expectedContent = "allright";
            const string expectedSize = "8 ";
            Assert.AreEqual(expectedSize, response.message);
            using (var fs = response.stream)
            {
                var bytes = new byte[fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                var content = Encoding.UTF8.GetString(bytes);
                Assert.AreEqual(expectedContent, content);
            }
        }

    }
}