using System;
using System.Collections.Generic;

namespace ConcurrentMatrixMultiplication
{
    /// <summary>
    /// Incapsulates methods of matrix generation
    /// </summary>
    public class MatrixGenerator
    {
        /// <summary>
        /// Generates matrix with random values
        /// </summary>
        /// <param name="amountOfRows">Amount of rows in new matrix</param>
        /// <param name="amountOfColumns">Amount of columns in new matrix</param>
        /// <returns>New matrix</returns>
        public static List<List<int>> GenerateRandomMatrix(int amountOfRows, int amountOfColumns)
        {
            var random = new Random();
            var result = new List<List<int>>();
            for (int i = 0; i < amountOfRows; ++i)
            {
                var newRow = new List<int>();
                for (int j = 0; j < amountOfColumns; ++j)
                {
                    newRow.Add(random.Next(-10, 10));
                }
                result.Add(newRow);
            }
            return result;
        }

        /// <summary>
        /// Creates square matrix with same values
        /// </summary>
        /// <param name="size">Size of matrix</param>
        /// <param name="values">Values in matrix</param>
        /// <returns>New matrix</returns>
        public static List<List<int>> GenerateMatrixByValue(int size, int value)
        {
            var matrix = new List<List<int>>();
            for (int i = 0; i < size; ++i)
            {
                var newRow = new List<int>();
                for (int j = 0; j < size; ++j)
                {
                    newRow.Add(value);
                }
                matrix.Add(newRow);
            }
            return matrix;
        }
    }
}
