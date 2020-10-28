using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace FTPClient
{
    class StreamHandler
    {
        TcpClient client;

        public StreamHandler(int port, string hostname)
        {
            client = new TcpClient(hostname, port);
        }

        public void Run()
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream) { AutoFlush = true};

            while (true)
            {
                var request = Console.ReadLine();
                writer.WriteLine(request);
                var response = reader.ReadLine();
                Console.WriteLine(response);
            }
        }
    }
}
