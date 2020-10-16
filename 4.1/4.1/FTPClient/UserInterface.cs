using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace FTPClient
{
    /// <summary>
    /// Coonsole user's interface
    /// </summary>
    class UserInterface
    {
        private readonly Client client;
        private readonly StreamReader reader;
        private readonly StreamWriter writer;

        public UserInterface(Client client, StreamReader reader, StreamWriter writer)
        {
            this.client = client;
            this.reader = reader;
            this.writer = writer;
        }

        public async void Run()
        {

        }
    }
}
