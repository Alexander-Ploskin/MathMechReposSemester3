using System;
using System.Diagnostics;

namespace ConcurrentMatrixMultiplication
{
    class Program
    {
        /// <summary>
        /// Measures boost by paralleling, using 10 random square matrix 
        /// </summary>
        /// <returns>Number of times that paralleling boosts multiplication</returns>
        private static double AnalyzeBoost()
        {
            double spentTimeSequentally = 0;
            double spentTimeConcurrently = 0;
            for (int i = 1; i <= 10; ++i)
            {
                var size = i * 70;
                var matrix1 = MatrixGenerator.GenerateRandomMatrix(size, size);
                var matrix2 = MatrixGenerator.GenerateRandomMatrix(size, size);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                SequentialMatrixMuliplier.Multiply(matrix1, matrix2);
                stopwatch.Stop();
                spentTimeSequentally += stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
                stopwatch.Start();
                ConcurrentMatrixMultiplier.Multiply(matrix1, matrix2);
                stopwatch.Stop();
                spentTimeConcurrently += stopwatch.ElapsedMilliseconds;
            }
            return spentTimeSequentally / spentTimeConcurrently;
        }

        /// <summary>
        /// Main func
        /// </summary>
        /// <param name="args">Args</param>
        static void Main(string[] args)
        {
            var matrix1 = MatrixFileIO.ReadMatrixFromFile(Environment.CurrentDirectory.TrimEnd(@"\bin\Debug\netcoreapp3.1".ToCharArray()) + @"ication\Matrix1.txt");
            var matrix2 = MatrixFileIO.ReadMatrixFromFile(Environment.CurrentDirectory.TrimEnd(@"\bin\Debug\netcoreapp3.1".ToCharArray()) + @"ication\Matrix2.txt");
            var matrix3 = ConcurrentMatrixMultiplier.Multiply(matrix1, matrix2);
            MatrixFileIO.WriteMatrixToFile(matrix3, Environment.CurrentDirectory.TrimEnd(@"\bin\Debug\netcoreapp3.1".ToCharArray()) + @"ication\Matrix3.txt");
            Console.WriteLine("Product of matrix1 and matrix2 wrote in the file");
            Console.WriteLine("Calculating...");
            Console.WriteLine($"Paralleling makes average { AnalyzeBoost() } times boost");
        }
    }
}
