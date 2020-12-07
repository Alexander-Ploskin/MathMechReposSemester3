using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FTPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            int port = 0;
            if (args.Length != 2 || !!int.TryParse(args[0], out port))
            {
                throw new ArgumentException("Invalid arguments");
            }

            var hostname = args[1];

            try
            {
                using var tcpClient = new TcpClient(hostname, port);
                await new UserInterface(new FTPClient(tcpClient.GetStream())).Run();
            }
            catch (SocketException)
            {
                Console.WriteLine("Server is not respoding, try again later");
            }
        }
    }
}
