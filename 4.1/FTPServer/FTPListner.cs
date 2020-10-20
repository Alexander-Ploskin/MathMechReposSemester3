using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Text;

namespace FTPServer
{
    /// <summary>
    /// Provides connection with FTP clients by TCP
    /// </summary>
    public class FTPListner
    {
        private readonly TcpListener tcpListener;

        /// <summary>
        /// Creates new FTP listner
        /// </summary>
        /// <param name="ip">Local IP-adress</param>
        /// <param name="port">Port to listening</param>
        public FTPListner(IPAddress ip, int port)
        {
            tcpListener = new TcpListener(ip, port);
        }

        private const string InputCommandPattern = "^[12] ..*";
        private const string ErrorMessage = "-1";

        /// <summary>
        /// Handles all incoming stream
        /// </summary>
        public async Task Run()
        {
            tcpListener.Start();
            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();
                var stream = client.GetStream();
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);

                try
                {
                    var request = await reader.ReadLineAsync();
                    if (request == null)
                    {
                        continue;
                    }
                    if (!Regex.IsMatch(request, InputCommandPattern))
                    {
                        await writer.WriteLineAsync(ErrorMessage + " Invalid request");
                        continue;
                    }

                    var commandCode = request[0];
                    var path = request.Substring(2);

                    switch (commandCode)
                    {
                        case '1':
                            {
                                await List(path, writer);
                                break;
                            }
                        case '2':
                            {
                                await Get(path, writer);
                                break;
                            }
                    }
                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Handles list requests
        /// </summary>
        /// <param name="path">Path of directory to listening</param>
        private async Task List(string path, StreamWriter writer)
        {
            try
            {
                var info = new DirectoryInfo(path);
                var filePaths = info.GetFiles().Select(info => Path.GetRelativePath(Directory.GetCurrentDirectory(), info.FullName)).ToList();
                var directoryPaths = info.GetDirectories().Select(info => Path.GetRelativePath(Directory.GetCurrentDirectory(), info.FullName)).ToList();
                var stringBuilder = new StringBuilder($"{filePaths.Count + directoryPaths.Count} ");
                stringBuilder.AppendJoin($" {false} ", filePaths);
                if (filePaths.Count != 0)
                {
                    stringBuilder.Append($" {false}");
                }

                stringBuilder.AppendJoin($" {true} ", directoryPaths);
                if (directoryPaths.Count != 0)
                {
                    stringBuilder.Append($" {true}");
                }

                await writer.WriteLineAsync(stringBuilder.ToString());
            }
            catch (Exception e)
            {
                await writer.WriteLineAsync(ErrorMessage + " " + e.Message);
            }
        }

        /// <summary>
        /// Handles get requests
        /// </summary>
        /// <param name="path">Path of file to download</param>
        /// <returns></returns>
        private async Task Get(string path, StreamWriter writer)
        {
            try
            {
                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    await writer.WriteLineAsync(new FileInfo(path).Length + " ");
                    await fileStream.CopyToAsync(writer.BaseStream);
                }
            }
            catch (Exception e)
            {
                await writer.WriteLineAsync(ErrorMessage + " " + e.Message);
            }
        }

    }
}
