using System.Net;
using System.Threading.Tasks;

namespace FTPServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const int port = 750;
            var iPAdress = IPAddress.Any;

            await new StreamHandler(port, iPAdress).Run();
        }
    }
}
