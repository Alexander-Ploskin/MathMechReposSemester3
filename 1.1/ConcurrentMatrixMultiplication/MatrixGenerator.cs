using System;
using System.Collections.Generic;

namespace ConcurrentMatrixMultiplication
{
    class MatrixGenerator
    {
        public static List<List<int>> Genetate(int amountOfRows, int amountOfColumns)
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
    }
}
