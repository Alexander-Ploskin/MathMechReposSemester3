using NUnit.Framework;
using System.Collections.Generic;

namespace ConcurrentMatrixMultiplication.Tests
{
    using ConcurrentMatrixMultiplication;
    class MatrixComparerTests
    {
        [Test]
        public void EmptyMatricesTest()
        {
            Assert.IsTrue(MatrixComparer.Compare(new List<List<int>>(), new List<List<int>>()));
        }

        [Test]
        public void SimpleCompareTest()
        {
            var matrix1 = new List<List<int>>() {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 } };
            var matrix2 = new List<List<int>>() {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 } };

            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix2));
            Assert.IsTrue(MatrixComparer.Compare(matrix2, matrix1));
        }

        [Test]
        public void FalseCompareTest()
        {
            var matrix1 = new List<List<int>>() {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 } };
            var matrix2 = new List<List<int>>() {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, -5, 6 } };

            Assert.IsFalse(MatrixComparer.Compare(matrix1, matrix2));
            Assert.IsFalse(MatrixComparer.Compare(matrix2, matrix1));
        }

        [Test]
        public void BigMatricesCompareTest()
        {
            const int size = 1000;
            var matrix1 = MatrixGenerator.GenerateMatrixByValue(size, 1);
            var matrix2 = MatrixGenerator.GenerateMatrixByValue(size, 1);
            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix2));
            Assert.IsTrue(MatrixComparer.Compare(matrix2, matrix1));
        }

        [Test]
        public void BigMatricesFalseCompareTest()
        {
            const int size = 1000;
            var matrix1 = MatrixGenerator.GenerateMatrixByValue(size, 1);
            var matrix2 = MatrixGenerator.GenerateMatrixByValue(size, 1);
            matrix2[990][87] = 30;
            Assert.IsFalse(MatrixComparer.Compare(matrix1, matrix2));
            Assert.IsFalse(MatrixComparer.Compare(matrix2, matrix1));
        }
    }
}
