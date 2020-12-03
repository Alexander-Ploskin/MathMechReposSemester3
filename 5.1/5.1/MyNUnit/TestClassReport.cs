using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace MyNUnit
{
    public class TestClassReport
    {
        public string AssemblyName { get; }

        public string ClassName { get; }

        public readonly List<SingleTestReport> reports;

        public TestClassReport(string assemblyName, string className)
        {
            AssemblyName = assemblyName;
            ClassName = className;
            reports = new List<SingleTestReport>();
        }

        public async Task ShowReport(TextWriter writer)
        {
            await writer.WriteLineAsync($"Test report for {ClassName} from {AssemblyName} :");
            var failed = 0;
            var passed = 0;
            var ignored = 0;
            foreach (var report in reports)
            {
                if (report.IngnoreCause != null)
                {
                    ignored++;
                    await writer.WriteLineAsync($"= {report.Name} ingored - {report.Message}");
                    continue;
                }
                else if (report.Passed == false)
                {
                    failed++;
                    await writer.WriteLineAsync($"- {report.Time} {report.Name} failed - {report.Message}");
                    continue;
                }
                passed++;
                await writer.WriteLineAsync($"+ {report.Time} {report.Name} passed");
            }

            await writer.WriteLineAsync($"Ignored: {ignored}");
            await writer.WriteLineAsync($"Executed: {failed + passed}");
            await writer.WriteLineAsync($"Failed: {failed}");
            await writer.WriteLineAsync($"Passed: {passed}");
        }

    }
}
