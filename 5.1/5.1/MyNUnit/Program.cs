using System;
using System.Reflection;
using System.Threading.Tasks;

namespace MyNUnit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var path = Console.ReadLine();
            await TestRunner.Run(path);
        }
    }
}
