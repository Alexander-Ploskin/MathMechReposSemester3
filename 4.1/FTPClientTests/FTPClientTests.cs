using NUnit.Framework;

namespace FTPClientTests
{
    using FTPClient;
    using System.IO;
    using System.Threading.Tasks;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Security.Cryptography;

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
            try
            {
                await fTPClient.ListAsync("path");
            }
            catch (Exception) { }
            stream.Position = 0;
            var request = await reader.ReadLineAsync();
            Assert.AreEqual("1 path", request);
        }

        [Test]
        public async Task CorrectGetRequestsTest()
        {
            try
            {
                await fTPClient.GetAsync("path", "pathtodownload", "name");
            }
            catch (Exception) { }
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
            var expectedResult = (0, new List<(string, bool)>());
            await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + response);
            stream.Position = 0;
            var result = await fTPClient.ListAsync("path");
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task NotEmptyDirResposeToListTest()
        {
            const string response = "2 path1 true path2 false";
            var expectedResult = (2, new List<(string, bool)>() { ("path1", true), ("path2", false)});
            await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + response);
            stream.Position = 0;
            var result = await fTPClient.ListAsync("path");
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public async Task GetEmptyFileTest()
        {
            const string recievedFilePath = TestDirectoryPath + "/RecievedEmptyFile.txt";
            const string sentFilePath = TestDirectoryPath + "/EmptyFile.txt";
            if (File.Exists(recievedFilePath))
            {
                File.Delete(recievedFilePath);
            }
            var size = new FileInfo(sentFilePath).Length;
            await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + size);
            var fileStream = new FileStream(sentFilePath, FileMode.Open);
            stream.Position = 0;
            await fileStream.CopyToAsync(writer.BaseStream);
            await fTPClient.GetAsync("path", TestDirectoryPath, "RecievedEmptyFile.txt");
            fileStream.Close();
            Assert.IsTrue(File.Exists(recievedFilePath));
            Assert.AreEqual(size, new FileInfo(recievedFilePath).Length);
        }

        [Test]
        public async Task GetNotEmptyFileTest()
        {
            var value = "allright";
            await writer.WriteLineAsync(SimulationOfRequestToCorrectStreamPosition + value.Length.ToString());
            stream.Position = 0;

            var newStream = new MemoryStream();
            var bytes = Encoding.UTF8.GetBytes(value);
            
            await newStream.CopyToAsync(writer.BaseStream);
            await newStream.WriteAsync(bytes);
            writer.BaseStream.Position = 0;

            await fTPClient.GetAsync("path", "pathtodownload", "name");
        }

    }
}