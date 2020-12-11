using System.IO;
using System.Threading.Tasks;
using System.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MyNUnitAttributes;
using System.Diagnostics;

namespace MyNUnit
{
    /// <summary>
    /// Provides unit testing from MyNUnit
    /// </summary>
    public static class TestRunner
    {
        private static ConcurrentQueue<TestClassReport> report;
        private static ConcurrentQueue<Assembly> assemblies;

        /// <summary>
        /// Runs tests in all assemblies in the directory and in all subdirectories
        /// </summary>
        /// <param name="path">Path to directory</param>
        /// <returns>Formatted report <see cref="TestClassReport"/></returns>
        public static IEnumerable<TestClassReport> RunTests(string path)
        {
            var files = Directory.EnumerateFiles(path, "*.dll", SearchOption.AllDirectories);
            assemblies = new ConcurrentQueue<Assembly>();
            Parallel.ForEach(files, LoadAssemblies);
            var classes = assemblies.Distinct().SelectMany(a => a.ExportedTypes).Where(t => t.IsClass);
            var testClasses = classes.Where(c => c.GetMethods().Any(m => m.GetCustomAttributes().Any(a => a is TestAttribute)));
            var invalidClasses = TestAnalyzer.AnalyzeTestAssembly(testClasses);
            if (invalidClasses.Count() != 0)
            {
                throw new InvalidAssemlyException(invalidClasses);
            }
            report = new ConcurrentQueue<TestClassReport>();

            Parallel.ForEach(testClasses, RunTests);
            return report;
        }

        private static void LoadAssemblies(string path)
        {
            assemblies.Enqueue(Assembly.LoadFrom(path));
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

            var tests = new ConcurrentQueue<TestInfo>();
            var beforeMethods = GetMethodsWithAttribute(className, typeof(BeforeAttribute));
            var afterMethods = GetMethodsWithAttribute(className, typeof(AfterAttribute));
            foreach (var method in GetMethodsWithAttribute(className, typeof(TestAttribute)))
            {
                tests.Enqueue(new TestInfo(className, method, beforeMethods, afterMethods, classReport));
            }
            Parallel.ForEach(tests, RunTest);
        }

        private class TestInfo
        {
            public TestInfo(Type className, MethodInfo method, IEnumerable<MethodInfo> beforeMethods,
                IEnumerable<MethodInfo> afterMethods, TestClassReport classReport)
            {
                ClassName = className;
                Method = method;
                BeforeMethods = beforeMethods;
                AfterMethods = afterMethods;
                ClassReport = classReport;
            }
            public Type ClassName { get; }
            public MethodInfo Method { get; }
            public IEnumerable<MethodInfo> BeforeMethods { get; }
            public IEnumerable<MethodInfo> AfterMethods { get; }

            public TestClassReport ClassReport { get; }
        }

        private static void RunTest(TestInfo info)
        {
            var instance = Activator.CreateInstance(info.ClassName);

            var attribute = (TestAttribute)info.Method.GetCustomAttribute(typeof(TestAttribute));

            if (attribute.Ignore != null)
            {
                info.ClassReport.reports.Add(new SingleTestReport(info.Method.Name, attribute.Ignore));
                return;
            }

            ExecuteStaticMethods(info.ClassName, typeof(BeforeClassAttribute));
            foreach (var method in info.BeforeMethods)
            {
                method.Invoke(instance, null);
            }

            Exception actual = null;
            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                info.Method.Invoke(instance, null);
                stopwatch.Stop();
            }
            catch (TargetInvocationException e)
            {
                stopwatch.Stop();
                actual = e.InnerException;
            }
            finally
            {
                info.ClassReport.reports.Add(new SingleTestReport(info.Method.Name, attribute.Expected, actual, stopwatch.Elapsed));
            }

            foreach (var method in info.AfterMethods)
            {
                method.Invoke(instance, null);
            }

            ExecuteStaticMethods(info.ClassName, typeof(AfterClassAttribute));
        }
    }
}
