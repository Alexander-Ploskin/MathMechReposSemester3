using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FTPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const int port = 8888;
            const string host = "localhost";
            var tcpClient = new TcpClient();
            var cuiOfFtpClient = new UserInterface(
                new Client(new StreamHandler(tcpClient.GetStream())),
                Console.In, Console.Out);
            await cuiOfFtpClient.Run();
        }
    }
}
