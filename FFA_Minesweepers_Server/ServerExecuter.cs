using System;

namespace TrueAxion.FFAMinesweepers
{
    class ServerExecutor
    {
        private const int listenPort = 13000;

        public static void Main(string[] args)
        {
            TcpServer tcpServer = new TcpServer(listenPort);
            tcpServer.Start();
        }
    }
}
