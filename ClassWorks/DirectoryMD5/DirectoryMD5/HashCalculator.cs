using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DirectoryMD5
{
    public static class HashCalculator
    {
        private static byte[] CalculateCheckSum(DirectoryInfo info)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(info.Name));

            foreach (var dir in info.GetDirectories().OrderBy(dir => dir.Name))
            { 
                hash.Concat(CalculateCheckSum(dir));
            }
            foreach (var file in info.GetFiles().OrderBy(file => file.Name))
            {
                byte[] buffer = new byte[1];
                GetHashFromFile((file, buffer));
                hash.Concat(buffer);
            }
            return hash;
        }

        private static void GetHashFromFile((FileInfo file, byte[] buffer) input)
        {
            using var fs = input.file.Open(FileMode.Open);
            input.buffer = MD5.Create().ComputeHash(fs);
        }

        private static byte[] CalculateCheckSumParralel(DirectoryInfo info)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(info.Name));
            foreach (var dir in info.GetDirectories().OrderBy(dir => dir.Name))
            {
                hash.Concat(CalculateCheckSumParralel(dir));
            }
            var buffers = new List<(FileInfo info, byte[] buffer)>();
            foreach(var file in info.GetFiles().OrderBy(file => file.Name))
            {
                buffers.Add((file, new byte[1]));
            }
            Parallel.ForEach(buffers, GetHashFromFile);
            foreach (var result in buffers)
            {
                hash.Concat(result.buffer);
            }

            return hash;
        }

        /// <summary>
        /// Calculates md5 hash of the directory
        /// </summary>
        /// <param name="path">Path of the directory</param>
        /// <returns>md5 hash</returns>
        public static byte[] CalculateCheckSum(string path) => CalculateCheckSum(new DirectoryInfo(path));

        /// <summary>
        /// Calculates md5 hash of the directory parralel
        /// </summary>
        /// <param name="path">Path of the directory</param>
        /// <returns>md5 hash</returns>
        public static byte[] CalculateCheckSumParralel(string path) => CalculateCheckSumParralel(new DirectoryInfo(path));

    }
}
