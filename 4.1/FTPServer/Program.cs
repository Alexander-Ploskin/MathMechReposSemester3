using System.Net;
using System.Threading.Tasks;
using System;

namespace FTPServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if ((args.Length != 1) || !int.TryParse(args[0], out var port))
            {
                throw new ArgumentException("Invalid arguments");
            }

            var ipAdress = IPAddress.Any;

            await new ConnectionProvider(port, ipAdress).Run();
        }
    }
}
