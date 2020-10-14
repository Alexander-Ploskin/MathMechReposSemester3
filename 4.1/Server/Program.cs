using System;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace FTPServer
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 8888;
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Listening on port {port}...");
            while (true)
            {
                using (var socket = listener.AcceptSocket())
                {
                    var stream = new NetworkStream(socket);
                    var streamReader = new StreamReader(stream);
                    var data = streamReader.ReadLine();
                    Console.WriteLine($"Received: {data}");
                    if (data == "break")
                    {
                        break;
                    }
                }
            }
            listener.Stop();

        }
    }
}
