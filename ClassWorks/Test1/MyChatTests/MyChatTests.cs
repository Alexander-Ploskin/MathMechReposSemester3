using NUnit.Framework;
using Moq;

namespace MyChatTests
{
    using MyChat;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class Tests
    {
        private MyChat chat;
        private Mock<TextWriter> textWriterMock;
        private Mock<TextReader> textReaderMock;
        private MemoryStream stream;

        [SetUp]
        public void Setup()
        {
            textWriterMock = new Mock<TextWriter>();
            textReaderMock = new Mock<TextReader>();
            stream = new MemoryStream();

            chat = new MyChat(stream, textWriterMock.Object, textReaderMock.Object);
        }

        [Test]
        public async Task GetMessageTest()
        {
            await chat.Run();
            stream.Write(Encoding.UTF8.GetBytes("My message"));
            textWriterMock.Verify(writer => writer.WriteLineAsync("My message"), Times.Once);
        }

        [Test]
        public async Task GetEmptyMessageTest()
        {
            await chat.Run();
            stream.Write(Encoding.UTF8.GetBytes(""));
            textWriterMock.Verify(writer => writer.WriteLineAsync(""), Times.Once);
        }

        [Test]
        public async Task SendMessageTest()
        {
            textReaderMock.SetupSequence(reader => reader.ReadLineAsync()).ReturnsAsync("My message");
            var streamReader = new StreamReader(stream);
            var sentMessage = await streamReader.ReadLineAsync();
            Assert.AreEqual("MyMessage", sentMessage);
        }

    }
}