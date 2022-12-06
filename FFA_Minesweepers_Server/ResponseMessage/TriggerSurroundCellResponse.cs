using System.Linq;
using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class TriggerSurroundCellResponse : IResponse
    {
        private readonly string receivedData;
        private Room room;

        public const char cellIdSeperator = ',';

        public TriggerSurroundCellResponse(string receivedData, Room room)
        {
            this.room = room;
            this.receivedData = receivedData;
        }

        public string CreateResponse()
        {
            string[] stringActionSet = ActionManager.GetStringActionSetFromString(receivedData, ActionManager.actionParameterSeperator);
            int[] actionSet = ActionManager.GetIntActionSetFromString(receivedData, ActionManager.actionParameterSeperator)
                                           .Take(stringActionSet.Count() - 1)
                                           .ToArray();
            int[] triggeredSurroundCellID = ActionManager.GetIntActionSetFromString(stringActionSet.Last(), cellIdSeperator);
            string responseData = null;

            for (int i = 0; i < triggeredSurroundCellID.Length; i++)
            {
                if (room.TriggeredCellID.Contains(triggeredSurroundCellID[i]))
                {
                    return null;
                }
            }

            room.TriggeredCellID = room.TriggeredCellID.Concat(triggeredSurroundCellID).ToHashSet();
            responseData = string.Join(ActionManager.actionParameterSeperator, actionSet);

            return responseData;
        }
    }
}
