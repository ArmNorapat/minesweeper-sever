using TrueAxion.FFAMinesweepers.GameRoom;

namespace TrueAxion.FFAMinesweepers.ResponseMessage
{
    public abstract class ResponseFactory
    {
        public abstract IResponse GetResponse(string receivedData, Room room, TcpClientHandler clientHandler);
    }
}
