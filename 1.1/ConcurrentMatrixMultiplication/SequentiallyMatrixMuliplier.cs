using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Runtime.Serialization.Formatters;
using System.Text;

namespace ConcurrentMatrixMultiplication
{
    class SequentiallyMatrixMuliplier
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
            var resut = new List<List<int>>();

            for (int indexOfRowInFirstMatrix = 0; indexOfRowInFirstMatrix < amountOfRowsInFirstMatrix; ++indexOfRowInFirstMatrix)
            {
                resut.Add(new List<int>());
                for (int indexOfColumnInSecondMatrix = 0; indexOfColumnInSecondMatrix < amountOfColumnsInSecondMatrix; ++indexOfColumnInSecondMatrix)
                {
                    var count = 0;
                    for (int indexOfElementInRowOfFirstMatrix = 0; indexOfElementInRowOfFirstMatrix < amountOfColumnsInFirstMatrix; ++indexOfElementInRowOfFirstMatrix)
                    {
                        count += firstMatrix[indexOfRowInFirstMatrix][indexOfElementInRowOfFirstMatrix] * secondMatrix[indexOfElementInRowOfFirstMatrix][indexOfColumnInSecondMatrix];
                    }
                    resut[indexOfRowInFirstMatrix].Add(count);
                }
            }
            return resut;
        }
    }
}
