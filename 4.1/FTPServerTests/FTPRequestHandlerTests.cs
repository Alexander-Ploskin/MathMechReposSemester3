using NUnit.Framework;

namespace FTPServerTests
{
    using FTPServer;
    using System.Text;

    public class Tests
    {
        const string TestsDataDir = "../../../../ServerTestsData";
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
        public void NotEmptyDirectoryTest()
        {
            string path = $"{TestsDataDir}/NotEmptyFolder";
            const string expectedResponse = "3 ..\\..\\..\\..\\ServerTestsData\\NotEmptyFolder\\File1.txt false  " +
                "..\\..\\..\\..\\ServerTestsData\\NotEmptyFolder\\Dir1 true ..\\..\\..\\..\\ServerTestsData\\NotEmptyFolder\\Dir2 true";
            var response = FTPRequestsHandler.HadleRequest("1 " + path);
            Assert.AreEqual(expectedResponse, response.message);
            Assert.AreEqual(null, response.stream);
        }

        [Test]
        public void GetTest()
        {
            string path = $"{TestsDataDir}/NotEmptyFolder/File1.txt";
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