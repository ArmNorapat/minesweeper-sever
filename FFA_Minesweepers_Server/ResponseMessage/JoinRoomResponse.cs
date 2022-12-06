using System.Linq;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class JoinRoomResponse : IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.ReadyToStart;
        private int actionIndex => (int)action;
        private int joinRoomActionIndex = (int)ActionManager.NetworkAction.JoinRoom;
        private string receivedData;
        private Room room;
        private TcpClientHandler clientHandler;

        public JoinRoomResponse(string receivedData, Room room, TcpClientHandler clientHandler)
        {
            this.receivedData = receivedData;
            this.room = room;
            this.clientHandler = clientHandler;
        }

        public string CreateResponse()
        {
            string name = ActionManager.GetStringActionSetFromString(receivedData, ActionManager.actionParameterSeperator).Last();
            clientHandler.ClientName = name;
            clientHandler.Server.RoomManager.JoinRoom(clientHandler);
            clientHandler.SendMessage($"{joinRoomActionIndex}|{clientHandler.Room.RoomID}");

            return $"{actionIndex}|{ActionManager.disable}";
        }
    }
}
