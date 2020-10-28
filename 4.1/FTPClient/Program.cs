using System;

namespace FTPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 750;
            const string hostname = "localhost";

            new StreamHandler(port, hostname).Run();
        }
    }
}
