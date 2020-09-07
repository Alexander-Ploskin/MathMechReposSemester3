using System;
using System.Collections.Generic;
using System.IO;

namespace ConcurrentMatrixMultiplication
{
    /// <summary>
    /// Incapsulated methods of reading matrices from file
    /// </summary>
    static class MatrixFileIO
    {
        /// <summary>
        /// Reads matrix from file
        /// </summary>
        /// <param name="path">Path of the file</param>
        /// <returns>Matrix representated by generic list</returns>
        public static List<List<int>> ReadMatrixFromFile(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var characters = sr.ReadToEnd();
                var fsd = ParseCharacters(characters);
                return fsd;
            }
        }

        /// <summary>
        /// Writes matrix to the file
        /// </summary>
        /// <param name="matrix">Matrix, that will be wrote to the file</param>
        /// <param name="path">Path of the new file</param>
        public static void WriteMatrixToFile(List<List<int>> matrix, string path)
        {
            try
            {
                using (var sw = new StreamWriter(path, false, System.Text.Encoding.Default))
                {
                    foreach (var row in matrix)
                    {
                        foreach (var element in row)
                        {
                            sw.Write(element.ToString() + " ");
                        }
                        sw.Write("\r\n");
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Invalid path");
            }
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
                            if (char.IsDigit(newToken))
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
                            throw new ApplicationException();
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
    }
}
