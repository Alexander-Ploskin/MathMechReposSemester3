using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;

namespace ConcurrentMatrixMultiplication
{
    class ConcurrentMatrixMultiplier
    {
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
            int amountOfThreads = Math.Min(baseAmountOfThreads, amountOfRowsInFirstMatrix);
            var threads = new Thread[amountOfThreads];
            var chunkSize = amountOfRowsInFirstMatrix / amountOfThreads + 1;
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

        private static void MultiplyChank(List<List<int>> firstMatrix, List<List<int>> secondMatrix, int firstRow, int lastRow, List<List<int>> buffer)
        {
            var amountOfRowsInFirstMatrix = firstMatrix.Count;
            var amountOfRowsInSecondMatrix = secondMatrix.Count;
            if (amountOfRowsInFirstMatrix == 0 && amountOfRowsInSecondMatrix == 0)
            {
                return;
            }
            var amountOfColumnsInFirstMatrix = firstMatrix[0].Count;
            var amountOfColumnsInSecondMatrix = secondMatrix[0].Count;
            if (amountOfColumnsInFirstMatrix != amountOfRowsInSecondMatrix)
            {
                throw new ApplicationException();
            }
            if (firstRow < 0 || lastRow < firstRow || lastRow > amountOfRowsInFirstMatrix)
            {
                throw new ApplicationException();
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
