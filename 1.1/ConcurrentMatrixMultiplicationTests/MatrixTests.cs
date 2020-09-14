using NUnit.Framework;

namespace ConcurrentMatrixMultiplicationTests
{
    using ConcurrentMatrixMultiplication;
    using System;
    using System.IO;

    class MatrixTests
    {
        [Test]
        public void NullValuesTest()
        {
            int[,] values = null;
            Assert.Throws<ArgumentNullException>(() => new Matrix(values));
        }

        [Test]
        public void InvalidPathTest() => Assert.Throws<ArgumentException>(() => new Matrix("sgggsdgfsdggfweg"));

        [Test]
        public void ReadFromFileTest()
        {
            var matrix1 = new Matrix(Environment.CurrentDirectory.TrimEnd(@"ConcurrentMatrixMultiplicationTests\bin\Debug\netcoreapp3.1".ToCharArray()) + $@"\ConcurrentMatrixMultiplication\Matrix1.txt");
            var matrix2 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 1, 2, 3 }
            });
            Assert.IsTrue(MatrixComparer.Compare(matrix1, matrix2));
        }

        [Test]
        public void WriteToFileTest()
        {
            var matrix1 = new Matrix(new int[,] {
                { 1, 2, 3 },
                { 1, 2, 3 }
            });
            string path = Environment.CurrentDirectory.TrimEnd(@"ConcurrentMatrixMultiplicationTests\bin\Debug\netcoreapp3.1".ToCharArray()) + $@"\ConcurrentMatrixMultiplication\Matrix1.txt";
            matrix1.WriteToFile(path);
            var matrix2 = new Matrix(path);
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
