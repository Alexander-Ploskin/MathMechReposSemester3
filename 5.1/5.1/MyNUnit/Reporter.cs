using System.IO;
using System.Threading.Tasks;

namespace MyNUnit
{
    public static class Reporter
    {
        public static async Task WriteReport(TestClassReport classReport, TextWriter writer)
        {
            await writer.WriteLineAsync($"Test report for {classReport.ClassName} from {classReport.AssemblyName} :");
            var failed = 0;
            var passed = 0;
            var ignored = 0;

            foreach (var report in classReport.reports)
            {
                var shortResult = "";
                var result = "";
                if (report.Ignored)
                {
                    ignored++;
                    shortResult = "=";
                    result = "ignored";
                }
                else if (report.Passed)
                {
                    passed++;
                    shortResult = "+";
                    result = "passed";
                }
                else
                {
                    failed++;
                    shortResult = "-";
                    result = "failed";
                }

                await writer.WriteLineAsync($"{shortResult} {report.Name} {result} - {report.Message}");
            }

            await writer.WriteLineAsync($"Ignored: {ignored}");
            await writer.WriteLineAsync($"Executed: {failed + passed}");
            await writer.WriteLineAsync($"Failed: {failed}");
            await writer.WriteLineAsync($"Passed: {passed}");

        }
    }
}
