using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class TriggerCellResponse : IResponse
    {
        private readonly string receivedData;
        private Room room;

        public TriggerCellResponse(string receivedData, Room room)
        {
            this.room = room;
            this.receivedData = receivedData;
        }

        public string CreateResponse()
        {
            int[] actionSet = ActionManager.GetIntActionSetFromString(receivedData, ActionManager.actionParameterSeperator);
            string responseData = null;

            int cellID = ActionManager.GetValidCellID(room.TriggeredCellID, actionSet);

            if (cellID != ActionManager.InvalidValue)
            {
                room.TriggeredCellID.Add(cellID);
                responseData = receivedData;
            }

            return responseData;
        }
    }
}
