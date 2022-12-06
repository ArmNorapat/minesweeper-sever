using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class CancelReadyToPlayResponse: IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.ReadyToStart;
        private int actionIndex => (int)action;
        private readonly string receivedData;
        private Room room;
        private TcpClientHandler clientHandler;

        public CancelReadyToPlayResponse(string receivedData, Room room, TcpClientHandler clientHandler)
        {
            this.receivedData = receivedData;
            this.room = room;
            this.clientHandler = clientHandler;
        }

        public string CreateResponse()
        {
            clientHandler.State = ClientState.NotReady;
            room.MasterClient.SendMessage($"{actionIndex}|{ActionManager.disable}");

            return receivedData;
        }
    }
}
