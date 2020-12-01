using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MyNUnit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Invalid appliaction arguments");
            }

            Console.WriteLine(args[0]);
        }
    }
}
