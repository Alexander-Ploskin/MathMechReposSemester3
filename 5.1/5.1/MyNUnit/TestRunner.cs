using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MyNUnit.Attributes;
using System.Diagnostics;

namespace MyNUnit
{
    /// <summary>
    /// Provides unit testing from MyNUnit
    /// </summary>
    public static class TestRunner
    {
        private static ConcurrentQueue<TestClassReport> report;

        /// <summary>
        /// Runs tests in all assemblies in the directory and in all subdirectories
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <returns>Formatted report <see cref="TestClassReport"/></returns>
        public static IEnumerable<TestClassReport> RunTests(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
            var classes = files.Select(Assembly.LoadFrom).Distinct().SelectMany(a => a.ExportedTypes).Where(t => t.IsClass);
            var testClasses = classes.Where(c => c.GetMethods().Any(m => m.GetCustomAttributes().Any(a => a is TestAttribute)));
            report = new ConcurrentQueue<TestClassReport>();

            Parallel.ForEach(testClasses, RunTests);
            return report;
        }

        /// <summary>
        /// Finds all methods with the inputed attribute
        /// </summary>
        /// <param name="className">Name of class in assembly</param>
        /// <param name="attributeName">Name of the attribute</param>
        /// <returns>Collection of methods</returns>
        private static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type className, Type attributeName)
            => className.GetMethods().Where(m => m.GetCustomAttributes().Any(a => a.GetType() == attributeName));

        /// <summary>
        /// Executes static methods with the attribute
        /// </summary>
        private static void ExecuteStaticMethods(Type classType, Type attributeType)
        {
            var methods = GetMethodsWithAttribute(classType, attributeType);

            foreach (var method in methods)
            {
                if (!method.IsStatic)
                {
                    throw new InvalidOperationException($"{method} must be static");
                }

                method.Invoke(null, null);
            }
        }

        /// <summary>
        /// Runs tests in the class and writes result into the report
        /// </summary>
        private static void RunTests(Type className)
        {
            var classReport = new TestClassReport(className.Assembly.FullName.Split(' ')[0].TrimEnd(','), className.Name);
            report.Enqueue(classReport);
            ExecuteStaticMethods(className, typeof(BeforeClassAttribute));

            var instance = Activator.CreateInstance(className);

            var beforeMethods = GetMethodsWithAttribute(className, typeof(BeforeAttribute));
            var afterMethods = GetMethodsWithAttribute(className, typeof(AfterAttribute));

            foreach (var testMethod in GetMethodsWithAttribute(className, typeof(TestAttribute)))
            {
                foreach (var method in beforeMethods)
                {
                    method.Invoke(instance, null);
                }

                var attribute = (TestAttribute)testMethod.GetCustomAttribute(typeof(TestAttribute));

                if (attribute.Ignore != null)
                {
                    classReport.reports.Add(new SingleTestReport(testMethod.Name, attribute.Ignore));
                    continue;
                }

                Exception actual = null;
                var stopwatch = new Stopwatch();

                try
                {
                    stopwatch.Start();
                    testMethod.Invoke(instance, null);
                    stopwatch.Stop();
                }
                catch (TargetInvocationException e)
                {
                    actual = e.InnerException;
                }
                finally
                {
                    classReport.reports.Add(new SingleTestReport(testMethod.Name, attribute.Expected, actual, stopwatch.Elapsed));
                }

                foreach (var method in afterMethods)
                {
                    method.Invoke(instance, null);
                }

            }

            ExecuteStaticMethods(className, typeof(AfterClassAttribute));
        }

    }
}
