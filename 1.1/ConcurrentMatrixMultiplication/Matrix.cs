using System;
using System.Collections.Generic;
using System.IO;

namespace ConcurrentMatrixMultiplication
{
    /// <summary>
    /// Implementation of an integer matrix 
    /// </summary>
    public class Matrix
    {
        /// <summary>
        /// Values in the matrix
        /// </summary>
        private readonly int[,] values;

        /// <summary>
        /// Amount of rows in the matrix
        /// </summary>
        public int Height => values.GetLength(0);

        /// <summary>
        /// Amount of columns in the matrix
        /// </summary>
        public int Width => values.GetLength(1);

        /// <summary>
        /// Creates new matrix by values
        /// </summary>
        /// <param name="values">Values to new matrix</param>
        public Matrix(int[,] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException();
            }
            this.values = (int[,])values.Clone();
        }

        /// <summary>
        /// Creates the new matrix with values from the file
        /// </summary>
        /// <param name="fileName">Name of the file</param>
        public Matrix(string fileName)
        {
            try
            {
                using (var sr = new StreamReader(fileName))
                {
                    var characters = sr.ReadToEnd();
                    var elements = ParseCharacters(characters);
                    values = ConvertToArray(elements);
                }
            }
            catch (IOException)
            {
                throw new ArgumentException("Invalid path!");
            }
        }

        /// <summary>
        /// Converts list of lists to 2-dim array
        /// </summary>
        /// <param name="matrix">list of lists</param>
        /// <returns>2-dim array with same values</returns>
        private int[,] ConvertToArray(List<List<int>> matrix)
        {
            var amountOfRows = matrix.Count;
            var amountOfColumns = matrix[0].Count;
            var array = new int[amountOfRows, amountOfColumns];
            for (int i = 0; i < amountOfRows; ++i)
            {
                for (int j = 0; j < amountOfColumns; ++j)
                {
                    array[i, j] = matrix[i][j];
                }
            }
            return array;
        }


        /// <summary>
        /// Parses file matrix representation
        /// </summary>
        /// <param name="characters">Chatacters from fiile</param>
        /// <returns>Matrix representated by generic list</returns>
        private static List<List<int>> ParseCharacters(string characters)
        {
            int currentIndex = 0;
            var matrix = new List<List<int>>();

            var firstRow = ParseRow(characters, ref currentIndex);
            matrix.Add(firstRow);
            var amountOfColumns = firstRow.Count;
            while (currentIndex < characters.Length)
            {
                var newRow = ParseRow(characters, ref currentIndex);
                if (newRow.Count != amountOfColumns)
                {
                    throw new ArgumentException();
                }
                matrix.Add(newRow);
            }
            return matrix;
        }

        /// <summary>
        /// Parses row of matrix
        /// </summary>
        /// <param name="characters">Characters from file</param>
        /// <param name="currentIndex">Current place of reading</param>
        /// <returns>Row of new matrix</returns>
        private static List<int> ParseRow(string characters, ref int currentIndex)
        {
            int currentStatement = 0;
            var row = new List<int>();
            string newNumber = "";
            while (currentIndex < characters.Length)
            {
                var newToken = characters[currentIndex];
                currentIndex++;
                //FSA recognizer of matrix row
                switch (currentStatement)
                {
                    case 0:   //Waiting new number state
                        {
                            if (char.IsDigit(newToken) || newToken == '-')
                            {
                                newNumber += newToken;
                                currentStatement = 1;
                                if (currentIndex == characters.Length)
                                {
                                    row.Add(int.Parse(newNumber));
                                    return row;
                                }
                                break;
                            }
                            if (newToken == ' ')
                            {
                                break;
                            }
                            if (newToken == '\r')
                            {
                                currentStatement = 2;
                                break;
                            }
                            throw new ApplicationException("Incorrect file");
                        }
                    case 1:   //Inputing new number state
                        {
                            if (char.IsDigit(newToken))
                            {
                                newNumber += newToken;
                                if (currentIndex == characters.Length)
                                {
                                    row.Add(int.Parse(newNumber));
                                    return row;
                                }
                                break;
                            }
                            if (newToken == ' ')
                            {
                                row.Add(int.Parse(newNumber));
                                newNumber = "";
                                currentStatement = 0;
                                if (currentIndex == characters.Length)
                                {
                                    return row;
                                }
                                break;
                            }
                            if (newToken == '\r')
                            {
                                row.Add(int.Parse(newNumber));
                                newNumber = "";
                                currentStatement = 2;
                                break;
                            }
                            throw new ApplicationException();
                        }
                    case 2:  //Ending of row state
                        {
                            if (newToken == '\n')
                            {
                                return row;
                            }
                            throw new ApplicationException();
                        }
                    default:
                        {
                            throw new Exception();
                        }
                }
            }
            return row;
        }

        /// <summary>
        /// Writes matrix to the file
        /// </summary>
        /// <param name="matrix">Matrix, that will be wrote to the file</param>
        /// <param name="fileName">Path of the new file</param>
        public void WriteToFile(string fileName)
        {
            using (var sw = new StreamWriter(fileName, false, System.Text.Encoding.Default))
            {
                for (int i = 0; i < Height; ++i)
                {
                    for (int j = 0; j < Width; ++j)
                    {
                        sw.Write(values[i, j].ToString() + " ");
                    }
                    sw.Write("\r\n");
                }
            }
        }

        /// <summary>
        /// Generates matrix with random values
        /// </summary>
        /// <param name="amountOfRows">Inputed amount of rows</param>
        /// <param name="amountOfColumns">Inputed amount of columns</param>
        public Matrix(int amountOfRows, int amountOfColumns)
        {
            var random = new Random();
            var matrix = new List<List<int>>();
            for (int i = 0; i < amountOfRows; ++i)
            {
                var newRow = new List<int>();
                for (int j = 0; j < amountOfColumns; ++j)
                {
                    newRow.Add(random.Next(-10, 10));
                }
                matrix.Add(newRow);
            }
            values = ConvertToArray(matrix);
        }

        /// <summary>
        /// Creates matrix with inputed value
        /// </summary>
        /// <param name="amountOfRows">Inputed amount of rows</param>
        /// <param name="amountOfColumns">Inputed amount of columns</param>
        /// <param name="value">Inputed value</param>
        public Matrix(int amountOfRows, int amountOfColumns, int value)
        {
            var matrix = new List<List<int>>();
            for (int i = 0; i < amountOfRows; ++i)
            {
                var newRow = new List<int>();
                for (int j = 0; j < amountOfColumns; ++j)
                {
                    newRow.Add(value);
                }
                matrix.Add(newRow);
            }

            values = ConvertToArray(matrix);
        }


        /// <summary>
        /// Gives access to the values of the matrix
        /// </summary>
        /// <param name="numberOfRow">Number of row</param>
        /// <param name="numberOfColumn">Number of column</param>
        /// <returns>Value in inputed position</returns>
        public int this[int numberOfRow, int numberOfColumn]
        {
            get => values[numberOfRow, numberOfColumn];
            set => values[numberOfRow, numberOfColumn] = value;
        }

    }
}
