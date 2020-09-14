using NUnit.Framework;

namespace ConcurrentMatrixMultiplicationTests
{
    using ConcurrentMatrixMultiplication;
    using System;

    public class Tests
    {
        [Test]
        public void EmptyMatricesTest()
        {
            var matrix1 = new Matrix(new int[0, 0] { });
            var matrix2 = new Matrix(new int[0, 0] { });
            var matrix3 = MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix3, matrix1));
            var matrix4 = MatrixMultiplier.MultiplySequentally(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix4, matrix1));
        }

        [Test]
        public void IncorrectSizedMatricesTest()
        {
            var matrix1 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 } });
            var matrix2 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 } });
            Assert.Throws<ArgumentException>(() => MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2));
            Assert.Throws<ArgumentException>(() => MatrixMultiplier.MultiplySequentally(matrix1, matrix2));
        }

        [Test]
        public void SimpleMultiplicationTest()
        {
            var matrix1 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 4, 5, 6 } });
            var matrix2 = new Matrix(new int[,] {
                { 9, 8, 7 },
                { 6, 5, 4 },
                { 3, 2, 1 }});
            var matrix3 = MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2);
            var matrix4 = new Matrix(new int[,] {
                { 30, 24, 18 },
                { 84, 69, 54 } });
            Assert.IsTrue(MatrixComparer.Compare(matrix3, matrix4));
            var matrix5 = MatrixMultiplier.MultiplySequentally(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix5, matrix4));
        }

        [Test]
        public void BigNumbersMultiplicationTest()
        {
            var matrix1 = new Matrix(new int[,] {
                { 100, 200, 300 },
                { 400, 500, 600 } });
            var matrix2 = new Matrix(new int[,] {
                { 900, 800, 700 },
                { 600, 500, 400 },
                { 300, 200, 100 }});
            var matrix3 = MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2);
            var matrix4 = new Matrix(new int[,] {
                { 300000, 240000, 180000 },
                { 840000, 690000, 540000 } });
            Assert.IsTrue(MatrixComparer.Compare(matrix3, matrix4));
            var matrix5 = MatrixMultiplier.MultiplySequentally(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix5, matrix4));
        }

        [Test]
        public void NegativeNumbersMultiplicationTest()
        {
            var matrix1 = new Matrix(new int[,] {
                { -1, -2, -3 },
                { -4, -5, -6 } });
            var matrix2 = new Matrix(new int[,] {
                { 9, 8, 7 },
                { 6, 5, 4 },
                { 3, 2, 1 }});
            var matrix3 = MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2);
            var matrix4 = new Matrix(new int[,] {
                { -30, -24, -18 },
                { -84, -69, -54 } });
            Assert.IsTrue(MatrixComparer.Compare(matrix3, matrix4));
            var matrix5 = MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix5, matrix4));
        }

        [Test]
        public void BigMatricesMultiplicationTest()
        {
            const int size = 100;
            var matrix1 = new Matrix(size, size, 1);
            var matrix2 = new Matrix(size, size, 2);
            var matrix3 = new Matrix(size, size, size * 2);
            var matrix4 = MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2);
            var matrix5 = MatrixMultiplier.MultiplySequentally(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix4, matrix3));
            Assert.IsTrue(MatrixComparer.Compare(matrix5, matrix3));
        }

        [Test]
        public void SourceMatricesTest()
        {
            var values1 = new int[,] {
                { -1, -2, -3 },
                { -4, -5, -6 } };
            var values2 = new int[,] {
                { 9, 8, 7 },
                { 6, 5, 4 },
                { 3, 2, 1 }};
            var matrix1 = new Matrix(values1);
            var matrix2 = new Matrix(values2);
            var matrix3 = new Matrix(values1);
            var matrix4 = new Matrix(values2);
            MatrixMultiplier.MultiplyConcurrentally(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix3));
            Assert.IsTrue(MatrixComparer.Compare(matrix2, matrix4));
            MatrixMultiplier.MultiplySequentally(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix3));
            Assert.IsTrue(MatrixComparer.Compare(matrix2, matrix4));
        }
    }
}