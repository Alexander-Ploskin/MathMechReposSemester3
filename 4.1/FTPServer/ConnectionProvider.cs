using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FTPServer
{
    /// <summary>
    /// Provider of the connection with clients
    /// </summary>
    public class ConnectionProvider
    {
        TcpListener tcpListener;

        /// <summary>
        /// Creates new connection provider with required port and ip
        /// </summary>
        /// <param name="port">Port to listening</param>
        /// <param name="iPAddress">ip address</param>
        public ConnectionProvider(int port, IPAddress iPAddress)
        {
            tcpListener = new TcpListener(iPAddress, port);
        }

        /// <summary>
        /// Eastablishes the connection with clients
        /// </summary>
        public async Task Run()
        {
            tcpListener.Start();

            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();
                Task.Run(async () => await HandleRequests(client.GetStream()));
            }
        }

        /// <summary>
        /// Cathes all requests from the stream
        /// </summary>
        /// <param name="stream">Client stream</param>
        private async Task HandleRequests(Stream stream)
        {
            var reader = new StreamReader(stream);
            await using var fileStream = File.Create("fsrgdsrgs");

            while (true)
            {
                var request = await reader.ReadLineAsync();
                Console.WriteLine(request);
                var response = FTPRequestsHandler.HadleRequest(request);
                var writer = new StreamWriter(stream) { AutoFlush = true };

                if (response.message != null)
                {
                    await writer.WriteLineAsync(response.message);
                }
                if (response.stream != null)
                {
                    await stream.CopyToAsync(writer.BaseStream);
                    stream.Close();
                }
            }
        }

    }
}
