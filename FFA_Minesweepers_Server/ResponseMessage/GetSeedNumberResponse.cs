using System;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class GetSeedNumberResponse : IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.GetSeedNumber;
        private int actionIndex => (int)action;
        private Room room;
        private TcpClientHandler clientHandler;

        public GetSeedNumberResponse(Room room, TcpClientHandler clientHandler)
        {
            this.room = room;
            this.clientHandler = clientHandler;
        }

        public string CreateResponse()
        {
            // Send seed to only requested client
            clientHandler.SendMessage($"{actionIndex}|{room.Seed}");

            return null;
        }
    }
}
