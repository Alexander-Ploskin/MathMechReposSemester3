using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FTPServer
{
    /// <summary>
    /// Listner of the input stream of FTP server
    /// </summary>
    class StreamHandler
    {
        private readonly TcpListener tcpListener;

        /// <summary>
        /// Creates new stream listner
        /// </summary>
        /// <param name="ip">Local IP adress</param>
        /// <param name="port">Port to listening</param>
        public StreamHandler(IPAddress ip, int port)
        {
            tcpListener = new TcpListener(ip, port);
        }

        /// <summary>
        /// Catches all stream redirects them to <see cref="FTPRequestsHandler"/>
        /// and sends responses
        /// </summary>
        public async Task Run()
        {
            tcpListener.Start();
            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();
                var stream = client.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                try
                {
                    var request = await reader.ReadLineAsync();
                    Console.WriteLine(request);
                    var response = FTPRequestsHandler.HadleRequest(request);
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
                catch
                {
                    continue;
                }
            }
        }

    }
}
