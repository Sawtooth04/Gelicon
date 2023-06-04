using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Threads.ThreadsContainer
{
    public interface IRoomsThreadsContainer
    {
        public RoomObserverThread CreateRoomObserverThread(string roomID, IHubCallerClients clients);

        public RoomObserverThread? FindRoomObserverThread(string roomID);
    }
}
