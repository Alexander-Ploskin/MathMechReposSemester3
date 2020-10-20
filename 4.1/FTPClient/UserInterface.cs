using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FTPClient
{
    class UserInterface
    {
        private readonly FTPClient ftpClient;
        private readonly TextWriter writer;
        private readonly TextReader reader;

        public UserInterface(FTPClient ftpClient, TextWriter writer, TextReader reader)
        {
            this.ftpClient = ftpClient;
            this.writer = writer;
            this.reader = reader;
        }

        private const string exitCode = "q";
        private const string listRequestPattern = "List request pattern: 1 <path: String>";
        private const string getREquestPattern = "Get request pattern: 2 <path: String>\n\t";
        private const string leaveCommand = "To leave enter";
        private const string listRequest = "List - show all files and folders in directory";
        private const string getRequest = "Get - download file";

        private async Task ShowIntrodunction()
        {
            writer.WriteLine()
        }

        public async Task Run()
        {
            Console.WriteLine
        }
    }
}
