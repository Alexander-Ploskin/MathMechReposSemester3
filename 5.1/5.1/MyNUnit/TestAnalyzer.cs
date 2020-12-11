using System.Reflection;
using MyNUnitAttributes;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System;


namespace MyNUnit
{
    /// <summary>
    /// Analyses methods with MyNUnit attributes in the assembly
    /// </summary>
    public static class TestAnalyzer
    {
        /// <summary>
        /// Finds errors in the test assembly
        /// </summary>
        /// <param name="assemblies">Assemblies to find errors</param>
        /// <returns>List of error reports <see cref="InvalidMethodReport">/></returns>
        public static IEnumerable<InvalidTestClassReport> AnalyzeTestAssembly(IEnumerable<Type> testClasses)
        {
            var report = new List<InvalidTestClassReport>();
            foreach (var testClass in testClasses)
            {
                var classReport = new InvalidTestClassReport(testClass.Name, new List<InvalidMethodReport>());
                var staticMethods = GetMethodsWithAttributes(testClass, new Type[2] {typeof(BeforeClassAttribute),
                    typeof(AfterClassAttribute)});
                var notStaticMethods = GetMethodsWithAttributes(testClass, new Type[3] {typeof(BeforeAttribute),
                    typeof(AfterAttribute), typeof(TestAttribute)});

                foreach (var method in staticMethods)
                {
                    var errors = FindErrors(method, true);
                    if (errors.Count != 0)
                    {
                        classReport.invalidMethods.Add(new InvalidMethodReport(method.Name, errors));
                    }
                }
                foreach (var method in notStaticMethods)
                {
                    var errors = FindErrors(method, false);
                    if (errors.Count != 0)
                    {
                        classReport.invalidMethods.Add(new InvalidMethodReport(method.Name, errors));
                    }
                }

                if (classReport.invalidMethods.Count() != 0)
                {
                    report.Add(classReport);
                }
            }

            return report;
        }

        private static List<string> FindErrors(MethodInfo method, bool shouldBeStatic)
        {
            var errors = new List<string>();
            if (method.IsStatic && !shouldBeStatic)
            {
                errors.Add("shouldn't be static");
            }
            if (!method.IsStatic && shouldBeStatic)
            {
                errors.Add("should be static");
            }
            if (method.ReturnType != typeof(void))
            {
                errors.Add("should be void");
            }
            if (method.GetParameters().Length != 0)
            {
                errors.Add("shouldn't has any parameters");
            }

            return errors;
        }

        private static IEnumerable<MethodInfo> GetMethodsWithAttributes(Type className, IEnumerable<Type> attributeName)
            => className.GetMethods().Where(m => m.GetCustomAttributes().Any(a => attributeName.Contains(a.GetType())));
    }
}
