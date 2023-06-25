
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsController
{
    public interface IRoomsThreadsController
    {
        public IRoomObserverThread CreateRoomObserverThread(string roomID, IHubCallerClients clients);

        public IRoomObserverThread? FindRoomObserverThread(string roomID);

        public void GetCurrentMusic(IClientProxy client, string roomID, string connectionID);

        public void SetPlayState(IClientProxy clients, string roomID, string connectionID);

        public void SetPauseState(IClientProxy clients, string roomID, string connectionID);

        public void SetNextMusic(IClientProxy clients, string roomID, string connectionID);

        public void SetPreviousMusic(IClientProxy clients, string roomID, string connectionID);
    }
}
