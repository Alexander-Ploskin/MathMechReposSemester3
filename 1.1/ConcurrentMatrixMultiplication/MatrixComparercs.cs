using System;
using System.Collections.Generic;
using System.Text;

namespace ConcurrentMatrixMultiplication
{
    class MatrixComparercs
    {
        public static bool Compare(List<List<int>> firstMatrix, List<List<int>> secondMatrix)
        {
            var amountOfRowsInFirstMatrix = firstMatrix.Count;
            var amountOfRowsInSecondMatrix = secondMatrix.Count;
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
