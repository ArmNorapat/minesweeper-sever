using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public class ConcreteResponseFactory : ResponseFactory
    {
        public override IResponse GetResponse(string receivedData, Room room, TcpClientHandler clientHandler)
        {
            ActionManager.NetworkAction action = ActionManager.GetActionFromString(receivedData);

            switch (action)
            {
                case ActionManager.NetworkAction.TriggerCell:
                    return new TriggerCellResponse(receivedData, room);

                case ActionManager.NetworkAction.FlagCell:
                    return new SimpleResponse(receivedData);

                case ActionManager.NetworkAction.TriggerSurroundCell:
                    return new TriggerSurroundCellResponse(receivedData, room);

                case ActionManager.NetworkAction.JoinRoom:
                    return new JoinRoomResponse(receivedData, room, clientHandler);

                case ActionManager.NetworkAction.UpdateRoom:
                    return new UpdateRoomResponse(room);

                case ActionManager.NetworkAction.ReadyToPlay:
                    return new ReadyToPlayResponse(receivedData, room, clientHandler);

                case ActionManager.NetworkAction.CancelReadyToPlay:
                    return new CancelReadyToPlayResponse(receivedData, room, clientHandler);

                case ActionManager.NetworkAction.StartGameRequest:
                    return new StartGameResponse(receivedData, room);

                case ActionManager.NetworkAction.ResetGame:
                    return new GetSeedNumberToAllResponse(room);

                case ActionManager.NetworkAction.GetSeedNumber:
                    return new GetSeedNumberResponse(room, clientHandler);

                case ActionManager.NetworkAction.ReadyToCountDown:
                    return new ReadyToCountDownResponse(room, clientHandler);

                case ActionManager.NetworkAction.LeaveRoom:
                    return new LeaveRoomResponse(room, clientHandler);
            }

            return new NoResponse();
        }
    }
}
