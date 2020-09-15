using System;
using System.Threading;

namespace ConcurrentMatrixMultiplication
{
    /// <summary>
    /// Incapsulation of the methods of the matrix multiplication
    /// </summary>
    public static class MatrixMultiplier
    {
        /// <summary>
        /// Multiplies 2 matrices sequentally
        /// </summary>
        /// <param name="firstMatrix">first matrix</param>
        /// <param name="secondMatrix">second matrix</param>
        /// <returns>New matrix</returns>
        public static Matrix MultiplySequentally(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix.Width != secondMatrix.Height)
            {
                throw new ArgumentException("Amount of columns in the first matrix must be equal to amount of rows in the second matrix");
            }
            var result = new int[firstMatrix.Height, secondMatrix.Width];

            for (int indexOfRowInFirstMatrix = 0; indexOfRowInFirstMatrix < firstMatrix.Height; ++indexOfRowInFirstMatrix)
            {
                for (int indexOfColumnInSecondMatrix = 0; indexOfColumnInSecondMatrix < secondMatrix.Width; ++indexOfColumnInSecondMatrix)
                {
                    var count = 0;
                    for (int indexOfElementInRowOfFirstMatrix = 0; indexOfElementInRowOfFirstMatrix < secondMatrix.Width; ++indexOfElementInRowOfFirstMatrix)
                    {
                        count += firstMatrix[indexOfRowInFirstMatrix, indexOfElementInRowOfFirstMatrix] * secondMatrix[indexOfElementInRowOfFirstMatrix, indexOfColumnInSecondMatrix];
                    }
                    result[indexOfRowInFirstMatrix, indexOfColumnInSecondMatrix] = count;
                }
            }
            return new Matrix(result);
        }

        /// <summary>
        /// Multiplies 2 matrices concurrentlly
        /// </summary>
        /// <param name="firstMatrix">first matrix</param>
        /// <param name="secondMatrix">second matrix</param>
        /// <returns>New matrix</returns>
        public static Matrix MultiplyConcurrentlly(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix.Width != secondMatrix.Height)
            {
                throw new ArgumentException("Amount of columns in the first matrix must be equal to amount of rows in the second matrix");
            }

            if (firstMatrix.Height == 0 && secondMatrix.Height == 0)
            {
                return new Matrix(new int[0, 0]);
            }

            int amountOfThreads = Math.Min(Environment.ProcessorCount, firstMatrix.Height);
            var threads = new Thread[amountOfThreads];
            var chunkSize = firstMatrix.Height / amountOfThreads + 1;

            var result = new int[firstMatrix.Height, secondMatrix.Width];
            for (int i = 0; i < amountOfThreads; ++i)
            {
                var localI = i;
                var startIndex = localI * chunkSize;
                if (startIndex >= firstMatrix.Height)
                {
                    break;
                }
                var lastIndex = Math.Min((localI + 1) * chunkSize, firstMatrix.Height);
                threads[i] = new Thread(() => MultiplyChunk(firstMatrix, secondMatrix, startIndex, lastIndex, result));
                if (lastIndex == firstMatrix.Height)
                {
                    break;
                }
            }

            foreach (var thread in threads)
            {
                if (thread != null)
                {
                    thread.Start();
                }
            }
            foreach (var thread in threads)
            {
                if (thread != null)
                {
                    thread.Join();
                }
            }
            return new Matrix(result);
        }

        /// <summary>
        /// Calculates chunk of matrix
        /// </summary>
        /// <param name="firstMatrix">first matrix</param>
        /// <param name="secondMatrix">first matrix</param>
        /// <param name="firstRow">low limit of calculation</param>
        /// <param name="lastRow">high limit of calculation</param>
        /// <param name="buffer">buffer for result</param>
        private static void MultiplyChunk(Matrix firstMatrix, Matrix secondMatrix, int firstRow, int lastRow, int[,] buffer)
        {
            if (firstRow < 0 || lastRow < firstRow || lastRow > firstMatrix.Height)
            {
                throw new ApplicationException("Invalid interval of computing");
            }
            for (int indexOfRowInFirstMatrix = firstRow; indexOfRowInFirstMatrix < lastRow; ++indexOfRowInFirstMatrix)
            {
                for (int indexOfColumnInSecondMatrix = 0; indexOfColumnInSecondMatrix < secondMatrix.Width; ++indexOfColumnInSecondMatrix)
                {
                    var count = 0;
                    for (int indexOfElementInRowOfFirstMatrix = 0; indexOfElementInRowOfFirstMatrix < firstMatrix.Width; ++indexOfElementInRowOfFirstMatrix)
                    {
                        count += firstMatrix[indexOfRowInFirstMatrix, indexOfElementInRowOfFirstMatrix] * secondMatrix[indexOfElementInRowOfFirstMatrix, indexOfColumnInSecondMatrix];
                    }
                    buffer[indexOfRowInFirstMatrix, indexOfColumnInSecondMatrix] = count;
                }
            }
        }

    }
}
