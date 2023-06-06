using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Threads.ThreadsContainer
{
    public class RoomsThreadsContainer : IRoomsThreadsContainer
    {
        private List<RoomObserverThread> roomObservers;

        public RoomsThreadsContainer()
        {
            roomObservers = new List<RoomObserverThread>();
        }

        public RoomObserverThread CreateRoomObserverThread(string roomID, IHubCallerClients clients)
        {
            RoomObserverThread? roomObserverThread;

            if ((roomObserverThread = FindRoomObserverThread(roomID)) == null)
            {
                roomObserverThread = new RoomObserverThread(roomID);
                roomObserverThread.clients = clients;
                roomObserverThread.addEmptyRoomHandler(EmptyRoomHandler);
                roomObservers.Add(roomObserverThread);
                roomObserverThread.Start();
            }
            return roomObserverThread;
        }

        public RoomObserverThread? FindRoomObserverThread(string roomID)
        {
            return roomObservers.Find(r => r.RoomID == roomID);
        }

        private void EmptyRoomHandler(RoomObserverThread thread)
        {
            roomObservers.Remove(thread);
        }
    }
}
