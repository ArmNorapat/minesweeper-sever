using System;
using System.Collections.Generic;
using System.Linq;
using TrueAxion.FFAMinesweepers.ResponseMessage;

namespace TrueAxion.FFAMinesweepers.GameRoom
{
    public class Room
    {
        public bool IsFull => PlayerAmounts >= maxPlayerAmounts;
        public bool IsInGame => MasterClient?.State == ClientState.InGame;
        public int RoomID { get; private set; }
        public HashSet<int> TriggeredCellID { get; set; }
        public int Seed { get; private set; }
        public TcpClientHandler MasterClient { get; private set; }
        public int PlayerAmounts => clientHandlers.Count;

        private const int maxPlayerAmounts = 4;
        private const char delimeter = '_';
        private Dictionary<int, TcpClientHandler> clientHandlers = new Dictionary<int, TcpClientHandler>();

        public Room(int roomID)
        {
            RoomID = roomID;
            TriggeredCellID = new HashSet<int>();
            MasterClient = null;
            ResetGame();
        }

        public void ResetGame()
        {
            TriggeredCellID.Clear();

            // generate new seed number
            Random r = new Random();
            Seed = r.Next(0, int.MaxValue);
        }

        public void AddNewClientHandler(TcpClientHandler clientHandler)
        {
            clientHandlers.Add(clientHandler.ClientID, clientHandler);

            if (MasterClient == null)
            {
                SetMasterClient(clientHandler);
            }

            BroadcastClientList();
            Console.WriteLine($"[TCP Server] client ({clientHandler.ClientID}) in room {RoomID}");
        }

        public void RemoveClientHandler(TcpClientHandler clientHandler)
        {
            if (clientHandler != null && clientHandlers.ContainsValue(clientHandler))
            {
                clientHandler.State = ClientState.NotReady;
                clientHandlers.Remove(clientHandler.ClientID);

                if (clientHandler.IsMasterClient)
                {
                    if (clientHandlers.Count > 0)
                    {
                        clientHandler.IsMasterClient = false;
                        SetMasterClient(clientHandlers.First().Value);
                    }
                    else
                    {
                        MasterClient = null;
                        ResetGame();
                    }
                }
                
                BroadcastClientList();
                Console.WriteLine($"[TCP Server] client ({clientHandler.ClientID}) exit room {RoomID}");
            }
        }

        public bool AreAllPlayersAtState(ClientState state)
        {
            return !clientHandlers.Any((x) => x.Value.State != state);
        }

        public void Broadcast(string responseData)
        {
            if (responseData != null)
            {
                foreach (KeyValuePair<int, TcpClientHandler> tch in clientHandlers)
                {
                    tch.Value.SendMessage(responseData);
                }
            }
        }

        public void SetAllClientsState(ClientState newState)
        {
            foreach (KeyValuePair<int, TcpClientHandler> tch in clientHandlers)
            {
                tch.Value.State = newState;
            }
        }

        private void BroadcastClientList()
        {
            Broadcast(new UpdateRoomResponse(this).CreateResponse());
        }

        public string GetClientIDsString()
        {
            return string.Join(",", clientHandlers.Select((x) => x.Value.ClientID));
        }

        public string GetClientNamesString()
        {
            return string.Join(",", clientHandlers.Select((x) => x.Value.ClientName));
        }

        private void SetMasterClient(TcpClientHandler clientHandler)
        {
            MasterClient = clientHandler;
            MasterClient.IsMasterClient = true;

            if (clientHandler.State != ClientState.InGame)
            { 
                MasterClient.State = ClientState.ReadyToPlay;
            }

            Console.WriteLine($"[TCP Server] Set new master client: client ({MasterClient.ClientID}) {MasterClient.IsMasterClient}");
        }
    }
}