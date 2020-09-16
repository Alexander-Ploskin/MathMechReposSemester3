using NUnit.Framework;

namespace ConcurrentMatrixMultiplicationTests
{
    using ConcurrentMatrixMultiplication;

    class MatrixComparerTests
    {
        [Test]
        public void EmptyMatricesTest()
        {
            var matrix1 = new Matrix(new int[0, 0] { });
            var matrix2 = new Matrix(new int[0, 0] { });
            Assert.IsTrue(MatrixComparer.Compare(matrix2, matrix1));
        }

        [Test]
        public void EqualMatricesTest()
        {
            var matrix1 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 } });
            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix1));
        }

        [Test]
        public void NotEqualMatricesTest()
        {
            var matrix1 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 } });
            var matrix2 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, -5, 6 } });
            Assert.IsFalse(MatrixComparer.Compare(matrix1, matrix2));
        }

        [Test]
        public void BigEqualMatricesTest()
        {
            var matrix1 = new Matrix(1000, 1);
            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix1));
        }

        [Test]
        public void BigNotEqualMatricesTest()
        {
            var matrix1 = new Matrix(1000, 1000, 1);
            var matrix2 = new Matrix(1000, 1000, 1);
            matrix2[900, 34] = -1;
            Assert.IsFalse(MatrixComparer.Compare(matrix1, matrix2));
        }

        [Test]
        public void NotSameSizedMatricesTest()
        {
            var matrix1 = new Matrix(100, 1);
            var matrix2 = new Matrix(101, 1);
            Assert.IsFalse(MatrixComparer.Compare(matrix1, matrix2));
        }

    }
}
