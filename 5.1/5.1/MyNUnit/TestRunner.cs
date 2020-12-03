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
        /// <summary>
        /// Runs tests in all assemblies in the directory and in all subdirectories
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <returns>Formatted report <see cref="TestClassReport"/></returns>
        public static IEnumerable<TestClassReport> RunTests(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
            var classes = files.Select(Assembly.LoadFrom).Distinct().SelectMany(a => a.ExportedTypes).Where(t => t.IsClass);
            var classesWithTests = classes.Where(c => c.GetMethods().Any(m => m.GetCustomAttributes().Any(a => a is TestAttribute)));

            var tests = new ConcurrentQueue<(Type, TestClassReport)>();
            foreach (var classType in classesWithTests)
            {
                tests.Enqueue((classType, new TestClassReport(classType.Assembly.FullName.Split(' ')[0].TrimEnd(','), classType.Name)));
            }
            Parallel.ForEach(tests, RunTests);
            return tests.Select(t => t.Item2);
        }

        /// <summary>
        /// Finds all methods with the inputed attribute
        /// </summary>
        /// <param name="classType">Name of class in assembly</param>
        /// <param name="attributeType">Name of the attribute</param>
        /// <returns>Collection of methods</returns>
        private static IEnumerable<MethodInfo> GetMethodsWithAttribute(Type classType, Type attributeType)
            => classType.GetMethods().Where(m => m.GetCustomAttributes().Any(a => a.GetType() == attributeType));

        /// <summary>
        /// Executes static methods with the attribute
        /// </summary>
        private static void ExecuteStaticMethods(Type classType, Type attributeType)
        {
            var methods = GetMethodsWithAttribute(classType, attributeType);

            foreach (var method in methods)
            {
                if (!method.IsStatic)
                    throw new InvalidOperationException($"{method} must be static");

                method.Invoke(null, null);
            }
        }

        /// <summary>
        /// Runs inputed tests and writes result into the report
        /// </summary>
        private static void RunTests((Type classType, TestClassReport report) testInput)
        {
            ExecuteStaticMethods(testInput.classType, typeof(BeforeClassAttribute));

            var instance = Activator.CreateInstance(testInput.classType);

            var beforeMethods = GetMethodsWithAttribute(testInput.classType, typeof(BeforeAttribute));
            var afterMethods = GetMethodsWithAttribute(testInput.classType, typeof(AfterAttribute));

            foreach (var testMethod in GetMethodsWithAttribute(testInput.classType, typeof(TestAttribute)))
            {
                foreach (var method in beforeMethods)
                {
                    method.Invoke(instance, null);
                }

                var attribute = (TestAttribute)testMethod.GetCustomAttribute(typeof(TestAttribute));

                if (attribute.Ignore != null)
                {
                    testInput.report.reports.Add(new SingleTestReport(testMethod.Name, attribute.Ignore));
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
                    testInput.report.reports.Add(new SingleTestReport(testMethod.Name, attribute.Expected, actual, stopwatch.Elapsed));
                }

                foreach (var method in afterMethods)
                {
                    method.Invoke(instance, null);
                }

            }

            ExecuteStaticMethods(testInput.classType, typeof(AfterClassAttribute));
        }

    }
}
