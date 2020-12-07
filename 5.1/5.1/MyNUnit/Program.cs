using System;
using System.Threading.Tasks;
using System.IO;

namespace MyNUnit
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Invalid appliaction arguments. You should enter only a path of the test directory");
            }

            var path = args[0];
            try
            {
                var reports = TestRunner.RunTests(args[0]);

                Console.WriteLine($"Execute tests in {path}");
                foreach (var report in reports)
                {
                    await Reporter.WriteReport(report, Console.Out);
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Counldn't find any directories in {path}");
            }
        }
    }
}
