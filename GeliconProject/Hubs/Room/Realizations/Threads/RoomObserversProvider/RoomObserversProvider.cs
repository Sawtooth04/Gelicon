using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomObserversProvider;
using GeliconProject.Storage.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads.RoomObserversProvider
{
    public class RoomObserversProvider : IRoomObserversProvider
    {
        private List<IRoomObserverThread> roomObservers;

        public RoomObserversProvider()
        {
            roomObservers = new List<IRoomObserverThread>();
        }

        public IRoomObserverThread CreateRoomObserverThread(string roomID, IHubCallerClients clients)
        {
            IRoomObserverThread? roomObserverThread;

            if ((roomObserverThread = FindRoomObserverThread(roomID)) == null)
            {
                roomObserverThread = new RoomObserverThread();
                roomObserverThread.RoomID = roomID;
                roomObserverThread.AddEmptyRoomHandler(EmptyRoomHandler);
                roomObservers.Add(roomObserverThread);
                roomObserverThread.Start();
            }
            return roomObserverThread;
        }

        public IRoomObserverThread? FindRoomObserverThread(string roomID)
        {
            return roomObservers.Find(r => r.RoomID == roomID);
        }

        private void EmptyRoomHandler(IRoomObserverThread thread)
        {
            roomObservers.Remove(thread);
        }
    }
}
