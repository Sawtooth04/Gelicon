using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.RoomThreadsFactory
{
    public interface IRoomThreadsFactory
    {
        public IRoomThread CreateRoomThread(string roomID, IRoomMusicPlayerSynchronizationMediator synchronizationMediator);
    }
}
