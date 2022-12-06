using System;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class ReadyToPlayResponse : IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.ReadyToStart;
        private int actionIndex => (int) action;
        private readonly string receivedData;
        private Room room;
        private TcpClientHandler clientHandler;

        public ReadyToPlayResponse(string receivedData, Room room, TcpClientHandler clientHandler)
        {
            this.receivedData = receivedData;
            this.room = room;
            this.clientHandler = clientHandler;
        }

        public string CreateResponse()
        {
            clientHandler.State = ClientState.ReadyToPlay;

            if (room.AreAllPlayersAtState(ClientState.ReadyToPlay))
            {
                Console.WriteLine("[TCP Server] All players are ready");
                room.MasterClient.SendMessage($"{actionIndex}|{ActionManager.enable}");
            }

            return receivedData;
        }
    }
}
