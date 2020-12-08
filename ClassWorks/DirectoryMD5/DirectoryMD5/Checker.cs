using System;
using System.IO;
using System.Security.Cryptography;

namespace DirectoryMD5
{
    public static class Checker
    {
        private static string CalculateCheckSum(DirectoryInfo info)
        {
            Console.WriteLine(info.GetFiles());
            return info.GetFiles().ToString();
        }

        public static string CalculateChecksum(string path)
        {
            return CalculateCheckSum(new DirectoryInfo(path));
        }
    }
}
