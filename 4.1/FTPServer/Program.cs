using System;
using System.IO;
using System.Net;

namespace FTPServer
{
    class Program
    {
        private static async void Main(string[] args)
        {
            const int port = 8888;
            await new FTPListner(IPAddress.Loopback, port).Run();
        }

    }
}
