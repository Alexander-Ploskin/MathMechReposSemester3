using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FTPServer
{
    /// <summary>
    /// Incapsulates handling of all availible requests to the FTP server
    /// </summary>
    public static class FTPRequestsHandler
    {
        public const string ErrorMessage = "-1";
        private const string InputCommandPattern = "^[12] ..*";

        public static (string message, FileStream stream) HadleRequest(string request)
        {
            if (!Regex.IsMatch(request, InputCommandPattern))
            {
                return (ErrorMessage + " Invalid request", null);
            }

            var commandCode = request[0];
            var path = request.Substring(2);

            try
            {
                switch (commandCode)
                {
                    case '1':
                        {
                            return (List(path), null);
                        }
                    case '2':
                        {
                            return Get(path);
                        }
                    default:
                        {
                            return (ErrorMessage + " Invalid request", null);
                        }
                }
            }
            catch (DirectoryNotFoundException)
            {
                return ($"{ErrorMessage} There are not directory, that you required", null);
            }
        }

        /// <summary>
        /// Handles list-requests to the server
        /// </summary>
        /// <param name="path">Relative path to the file</param>
        /// <returns>Count of files and directories and names</returns>
        private static string List(string path)
        {
            var info = new DirectoryInfo(path);
            var filePaths = info.GetFiles().Select(fileInfo => Path.GetRelativePath(Directory.GetCurrentDirectory(), fileInfo.FullName)).ToList();
            var directoryPaths = info.GetDirectories().Select(dirInfo => Path.GetRelativePath(Directory.GetCurrentDirectory(), dirInfo.FullName)).ToList();

            var output = $"{filePaths.Count + directoryPaths.Count}";
            foreach (var item in filePaths)
            {
                output += $" {item} false ";
            }
            foreach (var item in directoryPaths)
            {
                output += $" {item} true";
            }

            return output;
        }

        /// <summary>
        /// Handles get-requests to the server
        /// </summary>
        /// <param name="path">Relative path to the file</param>
        /// <returns>Size of the file and opened file stream</returns>
        /// <remarks>Caller should close the file after using</remarks>
        private static (string, FileStream) Get(string path)
        {
            var fileStream = new FileStream(path, FileMode.Open);
            var size = new FileInfo(path).Length + " ";
            return (size, fileStream);
        }

    }
}
