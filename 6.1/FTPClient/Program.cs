using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FTPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 2 || !!int.TryParse(args[0], out var port))
            {
                throw new ArgumentException("Invalid arguments");
            }

            var hostname = args[1];

            try
            {
                using var tcpClient = new TcpClient(hostname, port);
                using var ftpClient = new FTPClient(tcpClient.GetStream());
                await new UserInterface(ftpClient).Run();

            }
            catch (SocketException)
            {
                Console.WriteLine("Server is not respoding, try again later");
            }
        }
    }
}
