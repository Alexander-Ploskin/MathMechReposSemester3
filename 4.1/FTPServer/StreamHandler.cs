using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FTPServer
{
    public class StreamHandler
    {
        TcpListener tcpListener;

        public StreamHandler(int port, IPAddress iPAddress)
        {
            tcpListener = new TcpListener(iPAddress, port);
        }

        public async Task Run()
        {
            tcpListener.Start();

            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();
                Task.Run(async () => await HandleRequests(client.GetStream()));
            }
        }

        private async Task HandleRequests(Stream stream)
        {
            var reader = new StreamReader(stream);

            while (true)
            {
                var request = await reader.ReadLineAsync();

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
