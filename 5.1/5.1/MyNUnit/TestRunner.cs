using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace MyNUnit
{
    public static class TestRunner
    {
        private static ConcurrentQueue<Type> getClasses(string path)
        {
            var classes = new ConcurrentQueue<Type>();

            foreach (var file in Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories))
            {
                var assembly = Assembly.LoadFrom(file);
                foreach (var type in assembly.ExportedTypes)
                {
                    if (type.IsClass)
                    {
                        classes.Enqueue(type);
                    }
                }
            }

            return classes;
        }

        public static async Task<IEnumerable<TestClassReport>> Run(string path)
        {
            var reports = new ConcurrentQueue<TestClassReport>();
            var classes = getClasses(path);


        }
    }
}
