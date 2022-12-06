using System;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class ReadyToCountDownResponse : IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.GetCountDownTime;
        private int actionIndex => (int)action;
        private Room room;
        private TcpClientHandler clientHandler;

        private const int countDownSeconds = 3;

        public ReadyToCountDownResponse(Room room, TcpClientHandler clientHandler)
        {
            this.room = room;
            this.clientHandler = clientHandler;
        }

        public string CreateResponse()
        {
            clientHandler.State = ClientState.ReadyToCountDown;

            if (room.AreAllPlayersAtState(ClientState.ReadyToCountDown))
            {
                room.SetAllClientsState(ClientState.InGame);
                Console.WriteLine($"[TCP Server] Game Start in {countDownSeconds} seconds...");

                int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + countDownSeconds;

                return $"{actionIndex}|{unixTimestamp}";
            }

            return null;
        }
    }
}
