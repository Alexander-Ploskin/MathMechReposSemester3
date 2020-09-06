using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Schema;

namespace ConcurrentMatrixMultiplication
{
    static class MatrixReader
    {
        public static List<List<int>> ReadMatrix(string path)
        {
            using (var sr = new StreamReader(path))
            {
                var characters = sr.ReadToEnd();
                var fsd = ParseCharacters(characters);
                return fsd;
            }
        }

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

        private static List<int> ParseRow(string characters, ref int currentIndex)
        {
            int currentStatement = 0;
            var row = new List<int>();
            string newNumber = "";
            while (currentIndex < characters.Length)
            {
                var newToken = characters[currentIndex];
                currentIndex++;
                switch (currentStatement)
                {
                    case 0:
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
                    case 1:
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
                    case 2:
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
