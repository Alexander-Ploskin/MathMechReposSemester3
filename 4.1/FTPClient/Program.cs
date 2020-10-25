using System.Threading.Tasks;

namespace FTPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            const string hostname = "localhost";
            const int port = 8888;
            await new UserInterface(new FTPClient(hostname, port)).Run();
        }
    }
}
