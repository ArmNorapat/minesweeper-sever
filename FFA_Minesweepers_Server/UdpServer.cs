using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TrueAxion.FFAMinesweepers
{
    public class UdpServer
    {
        public struct UdpState
        {
            public UdpClient Client;
            public IPEndPoint EndPoint;
        }

        private Thread receiveMessageThread;
        private UdpClient udpClient;
        private UdpState udpState;

        public UdpServer(int port)
        {
            udpClient = new UdpClient(port);

            udpState = new UdpState();
            udpState.Client = udpClient;
            udpState.EndPoint = new IPEndPoint(IPAddress.Any, port);
        }

        public void Start()
        {
            Console.WriteLine("[UDP Server] Start running.");

            receiveMessageThread = new Thread(new ThreadStart(() => StartReceiveNewMessage(udpState)));
            receiveMessageThread.IsBackground = true;

            receiveMessageThread.Start();
        }

        public void CloseServer()
        {
            udpClient.Close();
            receiveMessageThread.Abort();

            Console.WriteLine("[UDP Server] Stop running.");
        }

        private void SendMessage(IPEndPoint ipEndPoint ,string newMessage)
        {
            Console.WriteLine($"[UDP Server] Send: {newMessage}\n");

            try
            {
                byte[] byteSend = Encoding.UTF8.GetBytes(newMessage);
                udpClient.Send(byteSend, byteSend.Length, ipEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }          
        }

        private void StartReceiveNewMessage(UdpState udpState)
        {
            Console.WriteLine("[UDP Server] Waiting for player...");
            udpClient.BeginReceive(OnMessageReceived, udpState);
        }

        private void OnMessageReceived(IAsyncResult asyncResult)
        {
            UdpState newUdpState = (UdpState)asyncResult.AsyncState;

            byte[] receiveBytes = newUdpState.Client.EndReceive(asyncResult, ref newUdpState.EndPoint);
            string receiveString = Encoding.UTF8.GetString(receiveBytes);

            Console.WriteLine($"[UDP Server] Received: {receiveString}");

            string message = receiveString == "PING" ? "PONG" : "ERROR";
            SendMessage(newUdpState.EndPoint, message);

            StartReceiveNewMessage(newUdpState);
        }
    }
}
