using GeliconProject.Hubs.Room.Realizations.Threads;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsProvider
{
    public interface IRoomsThreadsProvider
    {
        public IRoomObserverThread CreateRoomObserverThread(string roomID, IHubCallerClients clients);

        public IRoomObserverThread? FindRoomObserverThread(string roomID);
    }
}
