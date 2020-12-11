using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace MyNUnit
{
    /// <summary>
    /// Provides writign report of errors in the assembly
    /// </summary>
    public static class ErrorsReporter
    {
        /// <summary>
        /// Writes errors
        /// </summary>
        /// <param name="reports">Reports of errors in invalid test classes</param>
        /// <param name="writer">Wirter of the errors</param>
        public static async Task WriteErrors(IEnumerable<InvalidTestClassReport> reports, TextWriter writer)
        {
            foreach (var report in reports)
            {
                await writer.WriteLineAsync($"{report.Name}:");
                foreach (var method in report.InvalidMethods)
                {
                    foreach (var error in method.Errors)
                    {
                        await writer.WriteLineAsync($"{method.Name} {error}");
                    }
                }
            }
        }
    }
}
