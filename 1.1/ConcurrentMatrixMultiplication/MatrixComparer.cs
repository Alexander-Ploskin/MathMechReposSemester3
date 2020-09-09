using System.Collections.Generic;

namespace ConcurrentMatrixMultiplication
{
    /// <summary>
    /// Incapsulates methods of matrix comparing
    /// </summary>
    public class MatrixComparer
    {
        /// <summary>
        /// Compares two matrices
        /// </summary>
        /// <param name="firstMatrix">first matrix</param>
        /// <param name="secondMatrix">second matrix</param>
        /// <returns></returns>
        public static bool Compare(List<List<int>> firstMatrix, List<List<int>> secondMatrix)
        {
            var amountOfRowsInFirstMatrix = firstMatrix.Count;
            var amountOfRowsInSecondMatrix = secondMatrix.Count;
            if (amountOfRowsInFirstMatrix == 0 && amountOfRowsInSecondMatrix == 0)
            {
                return true;
            }
            var amountOfColumnsInFirstMatrix = firstMatrix[0].Count;
            var amountOfColumnsInSecondMatrix = secondMatrix[0].Count;
            if (amountOfColumnsInFirstMatrix != amountOfColumnsInSecondMatrix || amountOfRowsInFirstMatrix != amountOfRowsInSecondMatrix)
            {
                return false;
            }
            for (int i = 0; i < amountOfRowsInFirstMatrix; ++i)
            {
                for (int j = 0; j < amountOfColumnsInFirstMatrix; ++j)
                {
                    if (firstMatrix[i][j] != secondMatrix[i][j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
