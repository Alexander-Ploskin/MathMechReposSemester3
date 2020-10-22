using System.Net;
using System.Threading.Tasks;

namespace FTPServer
{
    class Program
    {
        private async static Task Main(string[] args)
        {
            const int port = 8888;
            await new StreamHandler(IPAddress.Loopback, port).Run();
        }
    }
}
