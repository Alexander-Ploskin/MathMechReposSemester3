using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DirectoryMD5
{
    /// <summary>
    /// Provides methods of statistic analyzis of the functions working time
    /// </summary>
    static class EfficiencyAnalyzer
    {
        /// <summary>
        /// Calculates expected time of function working on the user's input
        /// </summary>
        /// <param name="func">Function to analyze</param>
        /// <param name="parameters">Parameters to the function</param>
        /// <returns>Expected time of working</returns>
        public static TimeSpan CalculateExpectedTime(Func<string, byte[]> func, IEnumerable<string> parameters)
        {
            var elapsedTime = TimeSpan.Zero;
            var amountOfAttempts = 0;
            foreach (var param in parameters)
            {
                for (int i = 0; i < 5; ++i)
                {
                    amountOfAttempts++;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    func.Invoke(param);
                    stopwatch.Stop();
                    elapsedTime.Add(stopwatch.Elapsed);
                }
            }

            return elapsedTime.Divide(amountOfAttempts);
        }

        /// <summary>
        /// Calculates average deviation of expected time of the function on the user's input
        /// </summary>
        /// <param name="func">User's function</param>
        /// <param name="parameters">Params to work</param>
        /// <param name="expectedTime">Expected time of working</param>
        /// <returns>Average deviation</returns>
        public static TimeSpan CalculateAverageDeviation(Func<string, byte[]> func, IEnumerable<string> parameters, TimeSpan expectedTime)
        {
            var averageDeviation = TimeSpan.Zero;
            var amountOfAttempts = 0;
            foreach (var param in parameters)
            {
                for (int i = 0; i < 5; ++i)
                {
                    amountOfAttempts++;
                    var stopwatch = new Stopwatch();
                    stopwatch.Start();
                    func.Invoke(param);
                    stopwatch.Stop();
                    var deviation = stopwatch.Elapsed.Subtract(expectedTime);
                    averageDeviation += deviation > TimeSpan.Zero ? deviation : -deviation;
                }
            }

            return averageDeviation.Divide(amountOfAttempts);
        }
    }
}
