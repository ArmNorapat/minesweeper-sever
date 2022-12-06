using System;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class LeaveRoomResponse : IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.ReadyToStart;
        private int actionIndex => (int)action;
        private Room room;
        private TcpClientHandler clientHandler;

        public LeaveRoomResponse(Room room, TcpClientHandler clientHandler)
        {
            this.room = room;
            this.clientHandler = clientHandler;
        }

        public string CreateResponse()
        {
            room.RemoveClientHandler(clientHandler);
            string responseData = null;

            if (room.PlayerAmounts <= 1)
            {
                responseData = $"{actionIndex}|{ActionManager.disable}";
            }
            else if (room.AreAllPlayersAtState(ClientState.ReadyToPlay))
            {
                Console.WriteLine("[TCP Server] All players are ready");
                responseData = $"{actionIndex}|{ActionManager.enable}";
            }

            return responseData;
        }
    }
}
