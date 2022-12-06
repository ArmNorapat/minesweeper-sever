using System;
using System.Net;
using System.Net.Sockets;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers
{
    public class TcpServer
    {
        public readonly RoomManager RoomManager;

        private TcpListener server;
        private int listenPort;
        private int clientID;

        public TcpServer(int listenPort)
        {
            this.listenPort = listenPort;
            RoomManager = new RoomManager();
            clientID = 0;
        }

        public void Start()
        {
            server = new TcpListener(IPAddress.Any, listenPort);

            // Start listening for client requests.
            server.Start();
            Console.WriteLine($"[TCP Server] Start running at port: {listenPort}.");

            WaitForConnection();
        }

        public void WaitForConnection()
        {
            try
            {
                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("[TCP Server] Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    Console.WriteLine("[TCP Server] client Connected!");

                    TcpClientHandler clientHandler = new TcpClientHandler(this, client, clientID);
                    clientID++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                CloseServer();
            }
        }

        public void CloseServer()
        {
            // Stop listening for new clients.
            server.Stop();
        }
    }
}