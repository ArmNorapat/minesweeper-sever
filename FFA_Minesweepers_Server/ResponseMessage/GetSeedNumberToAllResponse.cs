using System;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class GetSeedNumberToAllResponse : IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.GetSeedNumber;
        private int actionIndex => (int)action;
        private Room room;

        public GetSeedNumberToAllResponse(Room room)
        {
            this.room = room;
        }

        public string CreateResponse()
        {
            room.ResetGame();
            return $"{actionIndex}|{room.Seed}";
        }
    }
}
