using System.Collections.Generic;
using System.Linq;

namespace TrueAxion.FFAMinesweepers
{
    public static class ActionManager
    {
        public enum NetworkAction
        {
            None,
            Instantiate,
            JoinRoom,
            UpdateRoom,
            GetNetworkId,
            ReadyToPlay,
            CancelReadyToPlay,
            ReadyToStart,
            StartGameRequest,
            StartGame,
            ResetGame,
            GetSeedNumber,
            ReadyToCountDown,
            GetCountDownTime,
            TriggerCell,
            FlagCell,
            TriggerSurroundCell,
            LeaveRoom,
            GameOver
        }

        public const int enable = 1;
        public const int disable = 0;
        public const int InvalidValue = -1;

        public const char actionParameterSeperator = '|';
        private const int actionIndex = 0;

        public static int[] GetIntActionSetFromString(string responseData, char seperator)
        {
            string[] actionSetString = GetStringActionSetFromString(responseData, seperator);
            int[] actionSet = new int[actionSetString.Count()];

            for (int i = 0; i < actionSetString.Length; i++)
            {
                if (!int.TryParse(actionSetString[i], out int action))
                {
                    action = InvalidValue;
                }

                actionSet[i] = action;
            }

            return actionSet;
        }

        public static string[] GetStringActionSetFromString(string responseData, char seperator)
        {
            return responseData.Split(seperator);
        }

        public static int GetValidCellID(HashSet<int> triggeredCellID, int[] actionSet)
        {
            NetworkAction action = (NetworkAction)actionSet[0];

            if (action == NetworkAction.TriggerCell || action == NetworkAction.FlagCell || action == NetworkAction.TriggerSurroundCell)
            {
                int cellID = actionSet.Last();

                if (!triggeredCellID.Contains(cellID))
                {
                    return cellID;
                }
            }

            return InvalidValue;
        }

        public static NetworkAction GetActionFromString(string responseData)
        {
            if (!int.TryParse(responseData.Split(actionParameterSeperator)[actionIndex], out int action))
            {
                action = 0;
            }

            return (NetworkAction)action;
        }
    }
}
