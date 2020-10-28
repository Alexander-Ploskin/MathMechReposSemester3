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
        private readonly FTPClient fTPClient;

        /// <summary>
        /// Creates new instance of UI by FTP client
        /// </summary>
        /// <param name="fTPClient">Client to work with the server</param>
        public UserInterface(FTPClient fTPClient)
        {
            this.fTPClient = fTPClient;
        }

        private const string ExitCommand = "q";
        private readonly string IntroductionOfExitCode = $"Enter {ExitCommand} to exit";
        private const string ListCommandPattern = "1 <path: String>\n\t path - path to directory, that you want to listening";
        private const string GetCommandPattern = "2 <path: String>\n\t path - path to file, that you want to download";
        private const string IntroductionOfAvailibleRequests = "Welcome to FTP client, here you can see availible commands:";
        private string IntrodunctionOfListRequest = $"List - to get list of files and directories in the server by path." +
            $" List command pattern:\n\t {ListCommandPattern}";
        private string IntrodunctionOfGetRequest = $"Get - to download file from the server by path." +
            $" Get command pattern:\n\t {GetCommandPattern}";
        private const string InputCommandPattern = "^[12] ..*";

        private IEnumerable<string> IntroductionMessages()
        {
            yield return IntroductionOfAvailibleRequests;
            yield return IntrodunctionOfListRequest;
            yield return IntrodunctionOfGetRequest;
            yield return IntroductionOfExitCode;
        }

        /// <summary>
        /// Runs conversation between user and FTP client
        /// </summary>
        public async Task Run()
        {
            foreach (var message in IntroductionMessages())
            {
                Console.WriteLine(message);
            }

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
                                var response = await fTPClient.ListAsync(path);
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
                                var response = await fTPClient.GetAsync(path, pathToDownload, name);
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
        private void ShowListResult((int size, IEnumerable<(string name, bool isDir)> items) response)
        {
            Console.WriteLine($"There are {response.size} items");
            foreach (var item in response.items)
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