using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThread;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.RoomThreadsFactory
{
    public interface IRoomThreadsFactory
    {
        public IRoomThread CreateRoomThread(string roomID, IRoomMusicPlayerSynchronizationMediator synchronizationMediator);
    }
}
