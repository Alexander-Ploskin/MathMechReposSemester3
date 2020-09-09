using System;
using System.Collections.Generic;

namespace ConcurrentMatrixMultiplication
{
    /// <summary>
    /// Incapsulated methods of sequential matrix multiplication
    /// </summary>
    public class SequentialMatrixMuliplier
    {
        /// <summary>
        /// Multiplies 2 matrices sequentally
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
                throw new ArgumentException("Amount of columns in first matrix must to be equal to amount of rows in second matrix");
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
