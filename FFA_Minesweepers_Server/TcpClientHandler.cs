using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TrueAxion.FFAMinesweepers.ResponseMessage;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers
{
    public class TcpClientHandler
    {
        public TcpClient Client { get; private set; }
        public ClientState State { get; set; }
        public Room Room { get; set; }
        public bool IsMasterClient { get; set; }
        public string ClientName { get; set; }
        public readonly int ClientID;
        public readonly TcpServer Server;

        private NetworkStream stream;
        private Thread handleClientThread;
        private ConcreteResponseFactory responseFactory;

        private const int responseBytes = 256;
        private const char delimeter = '_';

        public TcpClientHandler(TcpServer server, TcpClient client, int clientID)
        {
            Server = server;
            Client = client;
            ClientID = clientID;
            State = ClientState.NotReady;
            responseFactory = new ConcreteResponseFactory();

            // Get a stream object for reading and writing
            stream = client.GetStream();

            // send generated id to client
            SendMessage($"{(int)ActionManager.NetworkAction.GetNetworkId}|{clientID}");

            handleClientThread = new Thread(new ThreadStart(HandleClient));
            handleClientThread.Start();
        }

        public void HandleClient()
        {
            try
            {
                while (true)
                {
                    // Buffer for reading data
                    Byte[] bytes = new Byte[responseBytes];
                    string data = null;
                    int numberOfBytesRead = 0;

                    // Loop to receive all the data sent by the client.
                    do
                    {
                        // Translate data bytes to a UTF8 string.
                        numberOfBytesRead = stream.Read(bytes, 0, bytes.Length);
                        data = data + Encoding.UTF8.GetString(bytes, 0, numberOfBytesRead);
                    }
                    while (stream.DataAvailable);

                    if (numberOfBytesRead != 0)
                    {
                        Console.WriteLine($"[TCP Server] Action: {ActionManager.GetActionFromString(data)} Received: {data}");
                        string responseData = responseFactory.GetResponse(data, Room, this).CreateResponse();
                        Room?.Broadcast(responseData);   
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"[TCP Server] client ({ClientID}) Disconnected!");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
            finally
            {
                Dispose();
            }
        }

        public void SendMessage(string responseData)
        {
            try
            {
                byte[] msg = Encoding.UTF8.GetBytes(responseData + delimeter);

                NetworkStream stream = Client.GetStream();
                stream.Write(msg, 0, msg.Length);

                Console.WriteLine($"[TCP Server] Sent: {responseData}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e);
            }
        }

        public void Dispose()
        {
            Console.WriteLine($"[TCP Server] client ({ClientID}) Disconnected!");
            Room?.Broadcast(new LeaveRoomResponse(Room, this).CreateResponse());
            Room?.RemoveClientHandler(this);

            handleClientThread.Interrupt();
        }
    }
}