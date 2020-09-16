using NUnit.Framework;

namespace ConcurrentMatrixMultiplicationTests
{
    using ConcurrentMatrixMultiplication;
    using System;

    class MatrixTests
    {
        [Test]
        public void NullValuesTest()
        {
            int[,] values = null;
            Assert.Throws<ArgumentNullException>(() => new Matrix(values));
        }

        [Test]
        public void WorkWithFileTest()
        {
            var matrix1 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 1, 2, 3 }
            });
  
            matrix1.WriteToFile("Matrix1.txt");
            var matrix2 = new Matrix("Matrix1.txt");
            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix2));
        }

        [Test]
        public void RandomGenerationTest()
        {
            var matrix1 = new Matrix(2, 2);
            Assert.IsTrue(matrix1 != null && matrix1.Height == 2 && matrix1.Width == 2);
        }
        
        [Test]
        public void NotRandomGenerationTest()
        {
            var matrix1 = new Matrix(2, 2, 2);
            var matrix2 = new Matrix(new int[,] {
                { 2, 2 },
                { 2, 2 }
            });
            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix2));
        }
    }
}
