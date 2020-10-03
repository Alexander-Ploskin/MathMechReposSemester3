using System;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace SimpleFTP
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 8888;
            var listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            Console.WriteLine($"Listening on port {port}...");
            using (var socket = listener.AcceptSocket())
            {
                var stream = new NetworkStream(socket);
                var reader = new StreamReader(stream);
                var data = reader.ReadLine();
                Console.WriteLine($"Received: {data}");
                Console.WriteLine($"Sending \"Hi!\"");
                var writer = new StreamWriter(stream);
                writer.Write("Hi!");
                writer.Flush();
            }
            listener.Stop();
        }
    }
}
