using NUnit.Framework;

namespace FTPClientTests
{
    using FTPClient;
    using System.IO;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;

    public class FTPClientTests
    {
        private FTPClient fTPClient;
        private MemoryStream stream;
        private StreamReader reader;
        private StreamWriter writer;

        [SetUp]
        public void Setup()
        {
            stream = new MemoryStream();
            fTPClient = new FTPClient(stream);
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
        }

        private const string SimulationOfRequestToCorrectStreamPosition = "1 path  ";
        private const string TestDirectoryPath = "../../../../ClientTestsDir";

        [Test]
        public async Task CorrectListRequestsTest()
        {
            Assert.ThrowsAsync<ApplicationException>(() => fTPClient.ListAsync("path"));
            stream.Position = 0;
            var request = await reader.ReadLineAsync();
            Assert.AreEqual("1 path", request);
        }

        [Test]
        public async Task CorrectGetRequestsTest()
        {
            Assert.ThrowsAsync<ApplicationException>(() => fTPClient.GetAsync("path", "pathtodownload", "name"));
            stream.Position = 0;
            var request = await reader.ReadLineAsync();
            Assert.AreEqual("2 path", request);
        }

        [Test]
        public async Task ErrorServerResponseToListTest()
        {
            const string response = "-1 server has been broken";
            try
            {
                await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + response);
                stream.Position = 0;
                await fTPClient.ListAsync("path");
                Assert.Fail();
            }
            catch (ApplicationException e)
            {
                Assert.AreEqual(response, e.Message);
            }
        }

        [Test]
        public async Task ErrorServerResponseToGetTest()
        {
            const string response = "-1 server has been broken";
            try
            {
                await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + response);
                stream.Position = 0;
                await fTPClient.GetAsync("path", "pathtodownload", "name");
                Assert.Fail();
            }
            catch (ApplicationException e)
            {
                Assert.AreEqual(response, e.Message);
            }
        }

        [Test]
        [TestCase("1 ")]
        [TestCase("2 path path")]
        [TestCase("3 path true path true")]
        [TestCase("3 path true path")]
        [TestCase("1 path falsee")]
        [TestCase("1 path troe")]
        [TestCase("")]
        public async Task InvalidResponsesToListTest(string response)
        {
            try
            {
                await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + response);
                stream.Position = 0;
                await fTPClient.ListAsync("path");
                Assert.Fail();
            }
            catch (ApplicationException e)
            {
                Assert.AreEqual(FTPClient.InvalidResponseMessage, e.Message);
            }
        }

        [Test]
        [TestCase("size was lost")]
        [TestCase("2message")]
        [TestCase("0.1")]
        [TestCase("")]
        public async Task InvalidResponsesToGetTest(string response)
        {
            try
            {
                await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + response);
                stream.Position = 0;
                await fTPClient.GetAsync("path", "pathtodownload", "name");
                Assert.Fail();
            }
            catch (ApplicationException e)
            {
                Assert.AreEqual(FTPClient.InvalidResponseMessage, e.Message);
            }
        }

        [Test]
        public async Task EmptyDirResponseToListTest()
        {
            const string response = "0";
            var expectedResult = new List<(string, bool)>();
            await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + response);
            stream.Position = 0;
            var result = await fTPClient.ListAsync("path");
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task NotEmptyDirResponseToListTest()
        {
            const string response = "2 path1 true path2 false";
            var expectedResult = new List<(string, bool)>() { ("path1", true), ("path2", false)};
            await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + response);
            stream.Position = 0;
            var result = await fTPClient.ListAsync("path");
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task GetNotEmptyFileTest()
        {
            const string sentFilePath = TestDirectoryPath + "/NotEmptyFile.txt";
            const string recievedFileName = "RecievedNotEmptyFile.txt";
            using var fs = new FileStream(sentFilePath, FileMode.Open);
            await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + new FileInfo(sentFilePath).Length);
            await fs.CopyToAsync(writer.BaseStream);
            fs.Close();
            writer.BaseStream.Position = 0;
            await fTPClient.GetAsync("path", TestDirectoryPath, recievedFileName);

            const string receivedFilePath = TestDirectoryPath + "/" + recievedFileName;
            Assert.IsTrue(File.Exists(receivedFilePath));
            var recievedFileFs = new FileStream(receivedFilePath, FileMode.Open);
            Assert.AreEqual("allright", new StreamReader(recievedFileFs).ReadToEnd());
            recievedFileFs.Close();
        }

        [Test]
        public async Task GetEmptyFileTest()
        {
            const string sentFilePath = TestDirectoryPath + "/EmptyFile.txt";
            const string recievedFileName = "RecievedEmptyFile.txt";
            var fs = new FileStream(sentFilePath, FileMode.Open);
            await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + new FileInfo(sentFilePath).Length);
            await fs.CopyToAsync(writer.BaseStream);
            fs.Close();
            writer.BaseStream.Position = 0;
            await fTPClient.GetAsync("path", TestDirectoryPath, recievedFileName);

            const string recievedFilePath = TestDirectoryPath + "/" + recievedFileName;
            Assert.IsTrue(File.Exists(recievedFilePath));
            Assert.AreEqual(0, new FileInfo(recievedFilePath).Length);
        }

    }
}