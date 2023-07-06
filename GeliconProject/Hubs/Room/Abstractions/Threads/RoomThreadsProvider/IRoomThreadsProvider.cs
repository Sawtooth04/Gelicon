using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.RoomThreadsProvider
{
    public interface IRoomThreadsProvider
    {
        public IRoomThread CreateRoomObserverThread(string roomID, IRoomMusicPlayerSynchronizationMediator synchronizationMediator);

        public IRoomThread? FindRoomObserverThread(string roomID);
    }
}
