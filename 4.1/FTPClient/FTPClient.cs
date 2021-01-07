using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FTPClient
{
    /// <summary>
    /// Implementation of client that should to execute FTP requests
    /// </summary>
    public class FTPClient : IDisposable
    {
        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        /// <summary>
        /// Creates the new instance of FTPClient, that will work with the chosen stream
        /// </summary>
        /// <param name="stream">Stream </param>
        public FTPClient(Stream stream)
        {
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
        }

        public const string InvalidResponseMessage = "Invalid server response";
        private const string ListCode = "1";
        private const string GetCode = "2";

        /// <summary>
        /// Executes FTP request List
        /// </summary>
        /// <param name="path">Path of a directory on the server</param>
        public async Task<ICollection<(string, bool)>> ListAsync(string path)
        {
            var request = $"{ListCode} {path}";
            await writer.WriteLineAsync(request);
            var response = await reader.ReadLineAsync();

            if (response == null)
            {
                throw new ApplicationException(InvalidResponseMessage);
            }

            var splittedResponse = response.Split(' ');

            if (splittedResponse.Length == 0)
            {
                throw new ApplicationException(InvalidResponseMessage);
            }

            if (!int.TryParse(splittedResponse[0], out var size))
            {
                throw new ApplicationException(InvalidResponseMessage);
            }

            if (size < 0)
            {
                throw new ApplicationException(response);
            }

            var names = new List<(string, bool)>();
            try
            {
                for (int i = 1; i < size * 2; i += 2)
                {
                    var name = splittedResponse[i];
                    if (!bool.TryParse(splittedResponse[i + 1], out var isDir))
                    {
                        throw new ApplicationException(InvalidResponseMessage);
                    }
                    names.Add((name, isDir));
                }

                if (size != names.Count)
                {
                    throw new ApplicationException(InvalidResponseMessage);
                }

                return names;
            }
            catch (IndexOutOfRangeException)
            {
                throw new ApplicationException(InvalidResponseMessage);
            }
        }

        /// <summary>
        /// Executes FTP get request to the server
        /// </summary>
        /// <param name="path">Path of a file on the server</param>
        /// <param name="pathToDownload">Path to save a file on the client machine</param>
        /// <param name="name">Name of a saved file</param>
        public async Task<string> GetAsync(string path, string pathToDownload, string name)
        {
            var request = $"{GetCode} {path}";
            await writer.WriteLineAsync(request);
            var response = await reader.ReadLineAsync();
            if (response == null)
            {
                throw new ApplicationException(InvalidResponseMessage);
            }
            var splittedResponse = response.Split(' ', 2);
            if (!long.TryParse(splittedResponse[0], out var size))
            {
                throw new ApplicationException(InvalidResponseMessage);
            }

            if (size < 0)
            {
                throw new ApplicationException(response);
            }
            return await Download(size, pathToDownload, name);
        }

        /// <summary>
        /// Size of the buffer to dowloading (now i seted 10 KB, but I'm not sure, that it is optimal)
        /// </summary>
        private const int BufferSize = 10 * 1024;

        /// <summary>
        /// Downloads a file from the stream
        /// </summary>
        private async Task<string> Download(long size, string pathToDownload, string name)
        {
            try
            {
                reader.BaseStream.Position -= size;
            }
            catch (NotSupportedException) { }
            if (!Directory.Exists(pathToDownload))
            {
                Directory.CreateDirectory(pathToDownload);
            }
            var fileName = @$"{pathToDownload}\{name}";
            await using var fileStream = File.Create(fileName);

            var buffer = new byte[BufferSize];
            for (int i = 0; i < size / BufferSize; ++i)
            {
                await reader.BaseStream.ReadAsync(buffer, 0, BufferSize);
                await fileStream.WriteAsync(buffer);
            }
            await reader.BaseStream.ReadAsync(buffer, 0, (int)size % BufferSize);
            await fileStream.WriteAsync(buffer, 0, (int)size % BufferSize);

            return fileName;
        }

        public void Dispose()
        {
            writer.Dispose();
            reader.Dispose();
        }
    }
}
