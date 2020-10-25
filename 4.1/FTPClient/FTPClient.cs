using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace FTPClient
{
    class FTPClient
    {
        private readonly TcpClient tcpClient;
        private readonly StreamWriter writer;
        private readonly StreamReader reader;

        public FTPClient(string hostname, int port)
        {
            tcpClient = new TcpClient(hostname, port);
            var stream = tcpClient.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
        }

        const string InvalidResponseMessage = "Invalid response";

        public async Task<(int size, IEnumerable<(string path, bool isDirectory)> items)> ListAsync(string path)
        {
            var request = $"1 {path}";
            await writer.WriteLineAsync(request);
            var response = await reader.ReadLineAsync();
            if (response == null)
            {
                throw new ApplicationException(InvalidResponseMessage);
            }

            var splitedData = response.Split(' ');
            var stringSize = splitedData[0];
            if (!int.TryParse(stringSize, out var size))
            {
                throw new ApplicationException(InvalidResponseMessage);
            }
            if (size < 0)
            {
                throw new ArgumentException(response);
            }

            var output = new List<(string path, bool isDirectory)>();
            try
            {
                for (int i = 1; i < splitedData.Length; i += 2)
                {
                    var newPath = splitedData[i];
                    var isDirectoryString = splitedData[i + 1];
                    bool isDirectory = true;
                    if (!bool.TryParse(isDirectoryString, out isDirectory))
                    {
                        throw new ApplicationException(InvalidResponseMessage);
                    }
                    output.Add((newPath, isDirectory));
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new ApplicationException(InvalidResponseMessage);
            }

            return (size, output);
        }

        public async Task<string> GetAsync(string path)
        {
            var request = $"1 {path}";
            await writer.WriteLineAsync(request);
            var size = await RecieveSize();
            if (size < 0)
            {
                var errorMessage = await reader.ReadLineAsync();
                throw new ArgumentException(size + " " + errorMessage);
            }
            return await Download(size, path);
        }

        private async Task<long> RecieveSize()
        {
            var buffer = new byte[long.MaxValue.ToString().Length + 1];
            await reader.BaseStream.ReadAsync(buffer, 0, 2);
            var spaceIndex = 1;
            while (buffer[spaceIndex] != ' ')
            {
                spaceIndex++;
                await reader.BaseStream.ReadAsync(buffer, spaceIndex, 1);
            }

            if (!long.TryParse(buffer.ToString().ToCharArray(), out var size))
            {
                throw new ApplicationException("Invalid response");
            }

            return size;
        }

        const string PathToDownloads = "downloads";
        const int BufferSize = 2048;

        private async Task<string> Download(long size, string path)
        {
            if (size < 0)
            {
                throw new ArgumentException("Invalid path");
            }

            if (!Directory.Exists(PathToDownloads))
            {
                Directory.CreateDirectory(PathToDownloads);
            }

            var fileName = @$"{PathToDownloads}\{Path.GetFileName(path)}";
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
