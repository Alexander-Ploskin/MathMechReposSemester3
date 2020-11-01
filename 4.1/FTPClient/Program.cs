using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FTPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const int port = 750;
            const string hostname = "localhost";

            try
            {
                using (var tCPClient = new TcpClient(hostname, port))
                {
                    await new UserInterface(new FTPClient(tCPClient.GetStream())).Run();
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Server is not respoding, try again later");
            }
        }
    }
}
