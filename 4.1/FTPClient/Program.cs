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
                await new UserInterface(new FTPClient(port, hostname)).Run();
            }
            catch (SocketException)
            {
                Console.WriteLine("Server is not respoding, try again later");
            }
        }
    }
}
