using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThreadsFactory;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThreadsProvider;
using GeliconProject.Storage.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads.RoomThreadsProvider
{
    public class RoomThreadsProvider : IRoomThreadsProvider
    {
        private List<IRoomThread> roomObservers;
        private IRoomThreadsFactory roomThreadsFactory;

        public RoomThreadsProvider(IRoomThreadsFactory roomThreadsFactory)
        {
            roomObservers = new List<IRoomThread>();
            this.roomThreadsFactory = roomThreadsFactory;
        }

        public IRoomThread CreateRoomObserverThread(string roomID, IRoomMusicPlayerSynchronizationMediator synchronizationMediator)
        {
            IRoomThread roomObserverThread = roomThreadsFactory.CreateRoomThread(roomID, synchronizationMediator);
            roomObserverThread.AddEmptyRoomHandler(EmptyRoomHandler);
            roomObservers.Add(roomObserverThread);
            return roomObserverThread;
        }

        public IRoomThread? FindRoomObserverThread(string roomID)
        {
            return roomObservers.Find(r => r.RoomID == roomID);
        }

        private void EmptyRoomHandler(IRoomThread thread)
        {
            roomObservers.Remove(thread);
        }
    }
}
