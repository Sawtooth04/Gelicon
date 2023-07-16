
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThread;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsController
{
    public interface IRoomsThreadsController
    {
        public IRoomThread CreateRoomObserverThread(string roomID);

        public IRoomThread? FindRoomObserverThread(string roomID);

        public Task AddRoomMusic(IClientProxy clients, string roomID, string connectionID, string musicID);

        public Task GetRoomMusicList(IClientProxy clients, string roomID, string connectionID);

        public Task GetCurrentMusic(IClientProxy client, string roomID, string connectionID);

        public Task SetCurrentMusic(IClientProxy client, string roomID, string connectionID, string musicID);

        public Task SetPlayState(IClientProxy clients, string roomID, string connectionID);

        public Task SetPauseState(IClientProxy clients, string roomID, string connectionID);

        public Task SetNextMusic(IClientProxy clients, string roomID, string connectionID);

        public Task SetPreviousMusic(IClientProxy clients, string roomID, string connectionID);

        public Task SetAudioTime(IClientProxy clients, string roomID, string connectionID, double value);

        public Task DeleteRoomMusic(IClientProxy clients, string roomID, string connectionID, string musicID);

        public void CurrentTimePingResponse(string roomID, string connectionID, double value);

        public Task SetPlayNextState(IClientProxy clients, string roomID, string connectionID);

        public Task SetPlayLoopState(IClientProxy clients, string roomID, string connectionID);

        public Task SetAutoplayNextMusic(IClientProxy clients, string roomID, string musicID, string connectionID);

        public Task GetOnlineUsersList(IClientProxy client, string roomID);
    }
}
