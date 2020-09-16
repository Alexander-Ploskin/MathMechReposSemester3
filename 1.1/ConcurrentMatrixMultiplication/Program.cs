using System;
using System.Diagnostics;

namespace ConcurrentMatrixMultiplication
{
    class Program
    {
        private static (TimeSpan averageTime, TimeSpan averageDeviation) MatrixMultiplicationStopwatch(int size, int numberOfExperiments, Func<Matrix, Matrix, Matrix> multiply)
        {
            var stopwatch = new Stopwatch();
            var results = new TimeSpan[numberOfExperiments];
            for (int i = 0; i < numberOfExperiments; ++i)
            {
                results[i] = TimeSpan.Zero;
            }

            for (int i = 0; i < numberOfExperiments; ++i)
            {
                var matrix1 = new Matrix(size, size);
                var matrix2 = new Matrix(size, size);
                stopwatch.Restart();
                multiply(matrix1, matrix2);
                stopwatch.Stop();
                results[i] = stopwatch.Elapsed;
            }

            var averageTime = TimeSpan.Zero;
            for (int i = 0; i < numberOfExperiments; ++i)
            {
                averageTime += results[i];
            }
            averageTime /= numberOfExperiments;

            var averageDeviation = TimeSpan.Zero;
            for (int i = 0; i < numberOfExperiments; ++i)
            {
                var deviation = results[i] - averageTime;
                averageDeviation += deviation > TimeSpan.Zero ? deviation : -deviation;
            }
            averageDeviation /= numberOfExperiments;

            return (averageTime, averageDeviation);
        }
        /// <summary>
        /// Analizes time that spent by concurrent and sequental multiplication
        /// </summary>
        /// <returns>Number of times that paralleling boosts multiplication</returns>
        private static void AnalyzeBoost()
        {
            Console.WriteLine("Calculatig boost...");

            const int numberOfExperiments = 10;
            const int size = 600;
            var results = MatrixMultiplicationStopwatch(size, numberOfExperiments, MatrixMultiplier.MultiplySequentally);
            Console.WriteLine($"Matrices {size}×{size} multiplies sequentally for " +
                $"{results.averageTime.TotalSeconds} +/- {results.averageDeviation.TotalSeconds} seconds");

            results = MatrixMultiplicationStopwatch(size, numberOfExperiments, MatrixMultiplier.MultiplyConcurrently);
            Console.WriteLine($"Matrices {size}×{size} multiplies concurrently for " +
                $"{results.averageTime.TotalSeconds} +/- {results.averageDeviation.TotalSeconds} seconds");
        }

        static void Main(string[] args)
        {
            var firstMatrix = new Matrix(10, 10);
            var secondMatrix = new Matrix(10, 10);
            firstMatrix.WriteToFile("Matrix1");
            secondMatrix.WriteToFile("Matrix2");
            firstMatrix = new Matrix("Matrix1");
            secondMatrix = new Matrix("Matrix2");
            var resultMatrix = MatrixMultiplier.MultiplyConcurrently(firstMatrix, secondMatrix);
            resultMatrix.WriteToFile("Matrix3.txt");
            Console.WriteLine("Product of matrix1 and matrix2 wrote in the file");
            AnalyzeBoost();
        }
    }
}
