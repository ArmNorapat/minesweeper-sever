using System;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class StartGameResponse : IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.StartGame;
        private int actionIndex => (int)action;
        private readonly string receivedData;
        private Room room;

        public StartGameResponse(string receivedData, Room room)
        {
            this.room = room;
            this.receivedData = receivedData;
        }

        public string CreateResponse()
        {
            if (room.AreAllPlayersAtState(ClientState.InGame) || room.AreAllPlayersAtState(ClientState.ReadyToPlay))
            {
                return $"{actionIndex}|{ActionManager.enable}";
            }

            return $"{actionIndex}|{ActionManager.disable}";
        }
    }
}
