using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FTPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 8888;
            using (var client = new TcpClient("localhost", port))
            {
                while (true)
                {
                    Console.WriteLine($"Sending to port {port}...");
                    var stream = client.GetStream();
                    var writer = new StreamWriter(stream);
                    Console.WriteLine("Enter the message:");
                    var message = Console.ReadLine();
                    writer.WriteLine(message);
                    writer.Flush();
                }
            }
        }
    }
}
