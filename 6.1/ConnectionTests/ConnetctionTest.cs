using NUnit.Framework;
using FTPServer;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System;
using System.IO;

namespace ConnectionTests
{
    using FTPClient;
    public class ConnectionTest
    {
        private const int port = 49001;
        IPAddress ipAdress = IPAddress.Any;
        const string hostname = "localhost";

        [Test]
        public void FTPConnectionTest()
        { 
            new ConnectionProvider(port, ipAdress).Run();
            using var tcpClient = new TcpClient(hostname, port);
            using var ftpClient = new FTPClient(tcpClient.GetStream());
            Assert.ThrowsAsync<ApplicationException>(() => ftpClient.ListAsync("something"));
        }

        [Test]
        public async Task ServerResponseTest()
        {
            new ConnectionProvider(port, ipAdress).Run();
            using var tcpClient = new TcpClient(hostname, port);
            using var writer = new StreamWriter(tcpClient.GetStream()) { AutoFlush = true };
            using var reader = new StreamReader(tcpClient.GetStream());
            await writer.WriteLineAsync("something");

            var response = await reader.ReadLineAsync();
            Assert.AreEqual("-1 Invalid request", response);
        }

        [Test]
        public async Task ClientRequestTest()
        {
            var tcpListener = new TcpListener(ipAdress, port);
            tcpListener.Start();

            using var tcpClient = new TcpClient(hostname, port);
            using var ftpClient = new FTPClient(tcpClient.GetStream());

            using var client = await tcpListener.AcceptTcpClientAsync();
            using var reader = new StreamReader(client.GetStream());
            ftpClient.ListAsync("something");
            var request = await reader.ReadLineAsync();
            Assert.AreEqual("1 something", request);
            tcpListener.Stop();
        }

    }
}