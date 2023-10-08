using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FTPClient
{
    /// <summary>
    /// Console user interface for my ftp client <see cref="FTPClient"/>
    /// </summary>
    class UserInterface
    {
        private readonly FTPClient ftpClient;

        /// <summary>
        /// Creates new instance of UI by FTP client
        /// </summary>
        /// <param name="ftpClient">Client to work with the server</param>
        public UserInterface(FTPClient ftpClient)
        {
            this.ftpClient = ftpClient;
        }

        private const string ExitCommand = "q";
        private const string InputCommandPattern = "^[12] ..*";

        /// <summary>
        /// Runs conversation between user and FTP client
        /// </summary>
        public async Task Run()
        {
            Console.WriteLine("Welcome to FTP client, here you can see availible commands:");
            Console.WriteLine($"List - to get list of files and directories in the server by path." +
            " List command pattern:\n\t 1 < path: String >\n\t path - path to directory, that you want to listening");
            Console.WriteLine($"Get - to download file from the server by path." +
            " Get command pattern:\n\t 2 <path: String>\n\t path - path to file, that you want to download");
            Console.WriteLine($"Enter {ExitCommand} to exit");

            while (true)
            {
                var request = Console.ReadLine();
                if (request == ExitCommand)
                {
                    break;
                }
                if (!Regex.IsMatch(request, InputCommandPattern))
                {
                    Console.WriteLine("Invalid request!");
                    continue;
                }
                var splittedRequest = request.Split(' ');
                var commandId = splittedRequest[0];
                var path = splittedRequest[1];
                switch (commandId)
                {
                    case "1":
                        {
                            try
                            {
                                var response = await ftpClient.ListAsync(path);
                                ShowListResult(response);
                            }
                            catch (ApplicationException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                    case "2":
                        {
                            try
                            {
                                Console.WriteLine("Enter the path to download (relative): ");
                                var pathToDownload = Console.ReadLine();
                                Console.WriteLine("Enter the name: ");
                                var name = Console.ReadLine();
                                Console.WriteLine("Downloading...");
                                var response = await ftpClient.GetAsync(path, pathToDownload, name);
                                ShowGetResult(response);
                            }
                            catch (ApplicationException e)
                            {
                                Console.WriteLine(e.Message);
                            }
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Shows the server's response to list request
        /// </summary>
        /// <param name="response">Server's response</param>
        private void ShowListResult(ICollection<(string name, bool isDir)> response)
        {
            Console.WriteLine($"There are {response.Count} items");
            foreach (var item in response)
            {
                var type = item.isDir ? "directory" : "file";
                Console.WriteLine($"Path: {item.name} - {type}");
            }
        }

        /// <summary>
        /// Shows the server's response to get request
        /// </summary>
        /// <param name="response">Server's response</param>
        private void ShowGetResult(string response) => Console.WriteLine($"Downloading of {response} has been successfully completed");

    }
}