using System.Collections.Generic;

namespace TrueAxion.FFAMinesweepers.GameRoom
{
    public class RoomManager
    {
        private List<Room> rooms;
        private int lastRoomID;

        public RoomManager()
        {
            lastRoomID = 1;
            rooms = new List<Room>
            {
                new Room(lastRoomID)
            };
        }

        public void JoinRoom(TcpClientHandler clientHandler)
        {
            clientHandler.Room = GetEmptyRoom();
            clientHandler.Room.AddNewClientHandler(clientHandler);
        }

        private Room GetEmptyRoom()
        {
            Room emptyRoom = null;

            foreach (Room room in rooms)
            {
                if (!room.IsFull && !room.IsInGame)
                {
                    if (room.PlayerAmounts != 0)
                    {
                        return room;
                    }
                    else
                    {
                        emptyRoom = room;
                    }
                }
            }

            if (emptyRoom == null)
            {
                lastRoomID++;

                emptyRoom = new Room(lastRoomID);
                rooms.Add(emptyRoom);
            }

            return emptyRoom; 
        }
    }
}
