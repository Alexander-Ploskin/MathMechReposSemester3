using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyChat
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length == 2)
            {
                if (!int.TryParse(args[1], out var port))
                {
                    throw new ArgumentException("Invalid command line arguments");
                }
                var tcpClient = new TcpClient(args[0], port);
                var chat = new MyChat(tcpClient.GetStream(), Console.Out, Console.In);
                await chat.Run();
            }
            else if (args.Length == 1)
            {
                if (!int.TryParse(args[0], out var port))
                {
                    throw new ArgumentException("Invalid command line arguments");
                }
                var tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                var client = await tcpListener.AcceptTcpClientAsync();
                var chat = new MyChat(client.GetStream(), Console.Out, Console.In);
                await chat.Run();
            }
            throw new ArgumentException("Invalid command line arguments");
        }

    }
}
