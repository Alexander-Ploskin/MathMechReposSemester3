namespace ConcurrentMatrixMultiplication
{
    /// <summary>
    /// Incapsulates methods of matrix comparing
    /// </summary>
    public static class MatrixComparer
    {
        /// <summary>
        /// Compares two matrices
        /// </summary>
        /// <param name="firstMatrix">first matrix</param>
        /// <param name="secondMatrix">second matrix</param>
        /// <returns></returns>
        public static bool Compare(Matrix firstMatrix, Matrix secondMatrix)
        {
            if (firstMatrix.Width != secondMatrix.Width || firstMatrix.Height != secondMatrix.Height) 
            {
                return false;
            }
            for (int i = 0; i < firstMatrix.Height; ++i)
            {
                for (int j = 0; j < firstMatrix.Width; ++j)
                {
                    if (firstMatrix[i, j] != secondMatrix[i, j])
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}