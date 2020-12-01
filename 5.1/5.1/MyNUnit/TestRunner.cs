using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System;

namespace MyNUnit
{
    public static class TestRunner
    {
        public static async Task Run(string path)
        {
            foreach (var file in Directory.GetFiles(path, "*.dll"))
            {
                var name = new FileInfo(file).FullName;
                Console.WriteLine(name);
                var a = Assembly.Load(name);
                Console.WriteLine(a.FullName);
            }
        }
    }
}
