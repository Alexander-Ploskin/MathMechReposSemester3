using System;
using System.Collections.Generic;
using System.Threading;

namespace ConcurrentMatrixMultiplication
{
    /// <summary>
    /// Incapsulated methods of concurrent matrix multiplication
    /// </summary>
    class ConcurrentMatrixMultiplier
    {
        /// <summary>
        /// Multiplies 2 matrices concurrently
        /// </summary>
        /// <param name="firstMatrix">first matrix</param>
        /// <param name="secondMatrix">second matrix</param>
        /// <returns>New matrix</returns>
        public static List<List<int>> Multiply(List<List<int>> firstMatrix, List<List<int>> secondMatrix)
        {
            var amountOfRowsInFirstMatrix = firstMatrix.Count;
            var amountOfRowsInSecondMatrix = secondMatrix.Count;
            if (amountOfRowsInFirstMatrix == 0 && amountOfRowsInSecondMatrix == 0)
            {
                return new List<List<int>>();
            }
            var amountOfColumnsInFirstMatrix = firstMatrix[0].Count;
            var amountOfColumnsInSecondMatrix = secondMatrix[0].Count;
            if (amountOfColumnsInFirstMatrix != amountOfRowsInSecondMatrix)
            {
                throw new ApplicationException();
            }

            const int baseAmountOfThreads = 8;
            int amountOfThreads = Math.Min(baseAmountOfThreads, amountOfRowsInFirstMatrix);   //Calculating of amount of threads and chunk size
            var threads = new Thread[amountOfThreads];                                  //For little matrices amount of threads is amount of rows
            var chunkSize = amountOfRowsInFirstMatrix / amountOfThreads + 1;              //Bigger matrices can contain mor rows in one thread
                                                                                          //8 was setted as optimal after a lot of attempts with different matrices
            var results = new List<List<int>>[amountOfThreads];
            for (int i = 0; i < amountOfThreads; ++i)
            {
                results[i] = new List<List<int>>();
            }
            for (var i = 0; i < amountOfThreads; ++i)
            {
                var localI = i;
                var startIndex = localI * chunkSize;
                if (startIndex >= amountOfRowsInFirstMatrix)
                {
                    break;
                }
                var lastIndex = Math.Min((localI + 1) * chunkSize, amountOfRowsInFirstMatrix);
                threads[i] = new Thread(() => MultiplyChank(firstMatrix, secondMatrix, startIndex, lastIndex, results[localI]));
                if (lastIndex == amountOfRowsInFirstMatrix)
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

            //Linking of results from threads
            var result = new List<List<int>>();
            foreach (var item in results)
            {
                foreach (var row in item)
                {
                    result.Add(row);
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates chank of matrix
        /// </summary>
        /// <param name="firstMatrix">first matrix</param>
        /// <param name="secondMatrix">first matrix</param>
        /// <param name="firstRow">low limit of calculation</param>
        /// <param name="lastRow">high limit of calculation</param>
        /// <param name="buffer">buffer for calculated rows</param>
        private static void MultiplyChank(List<List<int>> firstMatrix, List<List<int>> secondMatrix, int firstRow, int lastRow, List<List<int>> buffer)
        {
            var amountOfRowsInFirstMatrix = firstMatrix.Count;
            var amountOfColumnsInFirstMatrix = firstMatrix[0].Count;
            var amountOfColumnsInSecondMatrix = secondMatrix[0].Count;
            if (firstRow < 0 || lastRow < firstRow || lastRow > amountOfRowsInFirstMatrix)
            {
                throw new ApplicationException("Invalid interval of computing");
            }
            for (int indexOfRowInFirstMatrix = firstRow; indexOfRowInFirstMatrix < lastRow; ++indexOfRowInFirstMatrix)
            {
                buffer.Add(new List<int>());
                for (int indexOfColumnInSecondMatrix = 0; indexOfColumnInSecondMatrix < amountOfColumnsInSecondMatrix; ++indexOfColumnInSecondMatrix)
                {
                    var count = 0;
                    for (int indexOfElementInRowOfFirstMatrix = 0; indexOfElementInRowOfFirstMatrix < amountOfColumnsInFirstMatrix; ++indexOfElementInRowOfFirstMatrix)
                    {
                        count += firstMatrix[indexOfRowInFirstMatrix][indexOfElementInRowOfFirstMatrix] * secondMatrix[indexOfElementInRowOfFirstMatrix][indexOfColumnInSecondMatrix];
                    }
                    buffer[buffer.Count - 1].Add(count);
                }
            }
        }
    }
}
