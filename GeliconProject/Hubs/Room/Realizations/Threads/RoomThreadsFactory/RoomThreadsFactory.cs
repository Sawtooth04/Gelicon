using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThreadsFactory;

namespace GeliconProject.Hubs.Room.Realizations.Threads.RoomThreadsFactory
{
    public class RoomThreadsFactory : IRoomThreadsFactory
    {
        public IRoomThread CreateRoomThread(string roomID, IRoomMusicPlayerSynchronizationMediator synchronizationMediator)
        {
            IRoomThread? roomObserverThread = new RoomThread(synchronizationMediator);
            roomObserverThread.RoomID = roomID;
            return roomObserverThread;
        }
    }
}
