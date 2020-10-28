using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FTPClient
{
    class FTPClient
    {
        private readonly TcpClient client;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        public FTPClient(int port, string hostname)
        {
            client = new TcpClient(hostname, port);
            var stream = client.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream) { AutoFlush = true };
        }

        const string InvalidResponseMessage = "Invalid server response";
        const string ListCode = "1";
        const string GetCode = "2";

        public async Task<(int size, IEnumerable<(string, bool)> names)> ListAsync(string path)
        {
            var request = $"{ListCode} {path}";
            await writer.WriteLineAsync(request);
            var response = await reader.ReadLineAsync();

            var splittedResponse = response.Split(' ');

            if (!int.TryParse(splittedResponse[0], out var size))
            {
                throw new ApplicationException(InvalidResponseMessage);
            }

            var names = new List<(string, bool)>();
            try
            {
                for (int i = 1; i < splittedResponse.Length; i += 2)
                {
                    var name = splittedResponse[i];
                    if (!bool.TryParse(splittedResponse[i + 1], out var isDir))
                    {
                        throw new ApplicationException(InvalidResponseMessage);
                    }
                    names.Add((name, isDir));
                }

                return (size, names);
            }
            catch (IndexOutOfRangeException)
            {
                throw new ApplicationException(InvalidResponseMessage);
            }
        }

        public async Task<string> GetAsync(string path, string pathToDownload, string name)
        {
            var request = $"{GetCode} {path}";
            await writer.WriteLineAsync(request);
            var stringSize = await reader.ReadLineAsync();
            if (!long.TryParse(stringSize, out var size) || size < 0)
            {
                throw new ApplicationException(InvalidResponseMessage);
            }

            return await Download(size, pathToDownload, name);
        }

        private const int BufferSize = 10 * 1024;

        private async Task<string> Download(long size, string pathToDownload, string name)
        {
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

    }
}
