using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System;

namespace FTPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string hostname = "localhost";
            const int port = 8888;
            ///await new UserInterface(new FTPClient(hostname, port)).Run();
            var tcpClient = new TcpClient(hostname, port);
            var stream = tcpClient.GetStream();
            var writer = new StreamWriter(stream);
            var reader = new StreamReader(stream);
            writer.WriteLine("rwewre");
            var response = reader.ReadLine();
            Console.WriteLine(response);
        }
    }
}
