using System;

namespace ConcurrentMatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrix1 = MatrixReader.ReadMatrix(Environment.CurrentDirectory.TrimEnd(@"\bin\Debug\netcoreapp3.1".ToCharArray()) + @"ication\Matrix1.txt");
            var matrix2 = MatrixReader.ReadMatrix(Environment.CurrentDirectory.TrimEnd(@"\bin\Debug\netcoreapp3.1".ToCharArray()) + @"ication\Matrix2.txt");
            var matrix3 = MatrixGenerator.Genetate(10, 5);
            var matrix4 = MatrixGenerator.Genetate(5, 3);
            var matrix5 = ConcurrentMatrixMultiplier.Multiply(matrix3, matrix4);
            var matrix6 = SequentiallyMatrixMuliplier.Multiply(matrix3, matrix4);
            Console.WriteLine(MatrixComparercs.Compare(matrix5, matrix6));
        }
    }
}
