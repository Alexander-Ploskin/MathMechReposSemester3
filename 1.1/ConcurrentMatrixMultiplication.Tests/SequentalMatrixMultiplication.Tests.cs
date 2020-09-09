using NUnit.Framework;
using System.Collections.Generic;

namespace ConcurrentMatrixMultiplication.Tests
{
    using ConcurrentMatrixMultiplication;
    using System;

    public class SequentalMatrixMultiplierTests
    {
        private List<List<int>> matrix1;
        private List<List<int>> matrix2;

        [SetUp]
        public void Setup()
        {
            matrix1 = new List<List<int>>();
            matrix2 = new List<List<int>>();
        }

        [Test]
        public void EmptyMatricesTest()
        {
            var matrix = SequentialMatrixMuliplier.Multiply(matrix1, matrix2);
            Assert.IsTrue(MatrixComparer.Compare(matrix, new List<List<int>>()));
        }

        [Test]
        public void IncorrectSizedMatricesTest()
        {
            matrix1 = new List<List<int>>() {
                new List<int> { 1, 2, 3 }, 
                new List<int> { 4, 5, 6 } };
            matrix2 = new List<List<int>>() { 
                new List<int> { 1, 2, 3 }, 
                new List<int> { 4, 5, 6 } };
            Assert.Throws<ArgumentException>(() => SequentialMatrixMuliplier.Multiply(matrix1, matrix2));
        }

        [Test]
        public void SimpleMultiplicationTest()
        {
            matrix1 = new List<List<int>>() {
                new List<int> { 1, 2, 3 },
                new List<int> { 4, 5, 6 } };
            matrix2 = new List<List<int>>() {
                new List<int> { 9, 8, 7 },
                new List<int> { 6, 5, 4 },
                new List<int> { 3, 2, 1 }};
            var matrix3 = SequentialMatrixMuliplier.Multiply(matrix1, matrix2);
            var matrix4 = new List<List<int>>() {
                new List<int> { 30, 24, 18 },
                new List<int> { 84, 69, 54 } };
            Assert.IsTrue(MatrixComparer.Compare(matrix3, matrix4));
        }

        [Test]
        public void BigNumbersMultiplicationTest()
        {
            matrix1 = new List<List<int>>() {
                new List<int> { 100, 200, 300 },
                new List<int> { 400, 500, 600 } };
            matrix2 = new List<List<int>>() {
                new List<int> { 900, 800, 700 },
                new List<int> { 600, 500, 400 },
                new List<int> { 300, 200, 100 }};
            var matrix3 = SequentialMatrixMuliplier.Multiply(matrix1, matrix2);
            var matrix4 = new List<List<int>>() {
                new List<int> { 300000, 240000, 180000 },
                new List<int> { 840000, 690000, 540000 } };
            Assert.IsTrue(MatrixComparer.Compare(matrix3, matrix4));
        }

        [Test]
        public void NegativeNumbersMultiplicationTest()
        {
            matrix1 = new List<List<int>>() {
                new List<int> { -1, -2, -3 },
                new List<int> { -4, -5, -6 } };
            matrix2 = new List<List<int>>() {
                new List<int> { 9, 8, 7 },
                new List<int> { 6, 5, 4 },
                new List<int> { 3, 2, 1 }};
            var matrix3 = SequentialMatrixMuliplier.Multiply(matrix1, matrix2);
            var matrix4 = new List<List<int>>() {
                new List<int> { -30, -24, -18 },
                new List<int> { -84, -69, -54 } };
            Assert.IsTrue(MatrixComparer.Compare(matrix3, matrix4));
        }

        [Test]
        public void BigMatricesMultiplicationTest()
        {
            const int size = 1000;
            matrix1 = MatrixGenerator.GenerateMatrixByValue(size, 1);
            matrix2 = MatrixGenerator.GenerateMatrixByValue(size, 2);
            var matrix3 = MatrixGenerator.GenerateMatrixByValue(size, 2000);
            Assert.IsTrue(MatrixComparer.Compare(SequentialMatrixMuliplier.Multiply(matrix1, matrix2), matrix3));
        }

    }
}