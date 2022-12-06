using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    class UpdateRoomResponse : IResponse
    {
        private readonly ActionManager.NetworkAction action = ActionManager.NetworkAction.UpdateRoom;
        private int actionIndex => (int)action;
        private Room room;

        public UpdateRoomResponse(Room room)
        {
            this.room = room;
        }

        public string CreateResponse()
        {
            int playerAmount = room.PlayerAmounts;
            return playerAmount == 0 ? null : $"{actionIndex}|{playerAmount}|{room.MasterClient.ClientID}|{room.GetClientIDsString()}|{room.GetClientNamesString()}";
        }
    }
}
