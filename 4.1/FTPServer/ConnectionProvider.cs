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
        private TcpListener tcpListener;

        /// <summary>
        /// Creates new connection provider with required port and ip
        /// </summary>
        /// <param name="port">Port to listening</param>
        /// <param name="iPAddress">ip address</param>
        public ConnectionProvider(int port, IPAddress ipAddress)
        {
            tcpListener = new TcpListener(ipAddress, port);
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
                Task.Run(async () => await HandleRequests(client));
            }
        }

        /// <summary>
        /// Cathes all requests from the stream
        /// </summary>
        /// <param name="stream">Client stream</param>
        private async Task HandleRequests(TcpClient client)
        {
            using (client)
            {
                using var stream = client.GetStream();
                using var reader = new StreamReader(stream);

                while (true)
                {
                    var request = await reader.ReadLineAsync();
                    var response = FTPRequestsHandler.HadleRequest(request);
                    using var writer = new StreamWriter(stream) { AutoFlush = true };

                    if (response.message != null)
                    {
                        await writer.WriteLineAsync(response.message);
                    }
                    if (response.stream != null)
                    {
                        await response.stream.CopyToAsync(writer.BaseStream);
                        response.stream.Close();
                    }
                }
            }
        }

    }
}
