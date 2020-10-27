using System;
using System.IO;
using System.Threading.Tasks;

namespace MyChat
{
    /// <summary>
    /// Sumple TCP chat, that can work with esteablished connection by the stream
    /// </summary>
    public class MyChat
    {
        private readonly StreamReader reader;
        private readonly StreamWriter writer;
        private readonly TextReader input;
        private readonly TextWriter output;

        /// <summary>
        /// Creates new chat by output, input source and stream
        /// </summary>
        /// <param name="stream">Connection stream</param>
        /// <param name="textWriter">Place to output messages</param>
        /// <param name="textReader">Place to input messages</param>
        public MyChat(Stream stream, TextWriter textWriter, TextReader textReader)
        {
            input = textReader;
            output = textWriter;
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
        }

        private const string IntroductionMessage = "Welcome to the my command line chat";
        private const string ExitCommand = "q";
        private string IntroductionOfExitCommand = $"To exit enter {ExitCommand}";

        /// <summary>
        /// Catches new messages from the stream
        /// </summary>
        private async Task GetMessages()
        {
            var newMessage = await reader.ReadLineAsync();
            if (newMessage != null)
            {
                output.WriteLine($"Host: {newMessage}");
            }
        }

        /// <summary>
        /// Run UI and sends all new messages
        /// </summary>
        public async Task Run()
        {
            Console.WriteLine(IntroductionMessage);
            Console.WriteLine(IntroductionOfExitCommand);
            await GetMessages();
            while (true)
            {
                var newMessage = await input.ReadLineAsync();
                if (newMessage == ExitCommand)
                {
                    return;
                }
                await writer.WriteLineAsync(newMessage);
            }
        }

    }
}
