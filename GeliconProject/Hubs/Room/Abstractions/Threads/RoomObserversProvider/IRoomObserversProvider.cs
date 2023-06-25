using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.RoomObserversProvider
{
    public interface IRoomObserversProvider
    {
        public IRoomObserverThread CreateRoomObserverThread(string roomID, IHubCallerClients clients);

        public IRoomObserverThread? FindRoomObserverThread(string roomID);
    }
}
