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
            for (int i = 1; i <= 12; ++i)
            {
                var size = i * 70;
                var matrix1 = new Matrix(size, size);
                var matrix2 = new Matrix(size, size);
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                MatrixMultiplier.MultiplySequentally(matrix1, matrix2);
                stopwatch.Stop();
                spentTimeSequentally += stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
                stopwatch.Start();
                MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2);
                stopwatch.Stop();
                spentTimeConcurrently += stopwatch.ElapsedMilliseconds;
            }
            return spentTimeSequentally / spentTimeConcurrently;
        }

        static void Main(string[] args)
        {
            var firstMatrix = new Matrix(PathGenerator.GetPathByName("Matrix1.txt"));
            var secondMatrix = new Matrix(PathGenerator.GetPathByName("Matrix2.txt"));
            var resultMatrix = MatrixMultiplier.MultiplyConcurrentally(firstMatrix, secondMatrix);
            resultMatrix.WriteToFile(PathGenerator.GetPathByName("Matrix3.txt"));
            Console.WriteLine("Product of matrix1 and matrix2 wrote in the file");
            Console.WriteLine("Calculating boost...");
            Console.WriteLine($"Paralleling makes average { AnalyzeBoost() } times boost");
        }
    }
}
