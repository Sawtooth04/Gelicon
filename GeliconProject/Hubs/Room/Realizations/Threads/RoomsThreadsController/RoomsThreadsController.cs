using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThread;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThreadsProvider;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsController;
using GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads.ThreadsController
{
    public class RoomsThreadsController : IRoomsThreadsController
    {
        private IRoomThreadsProvider roomObserversProvider;
        private IRoomMusicPlayerController roomMusicPlayerController;
        private IRoomMusicPlayerSynchronizationController synchronizationController;

        public RoomsThreadsController(IRoomThreadsProvider roomObserversProvider, IRoomMusicPlayerController roomMusicPlayerController, IRoomMusicPlayerSynchronizationController synchronizationController)
        {
            this.roomObserversProvider = roomObserversProvider;
            this.roomMusicPlayerController = roomMusicPlayerController;
            this.synchronizationController = synchronizationController;
        }

        public IRoomThread CreateRoomObserverThread(string roomID)
        {
            IRoomThread? roomObserverThread;

            if ((roomObserverThread = FindRoomObserverThread(roomID)) == null)
            {
                IRoomMusicPlayerSynchronizationMediator synchronizationMediator = new RoomMusicPlayerSynchronizationMediator();
                roomObserverThread = roomObserversProvider.CreateRoomObserverThread(roomID, synchronizationMediator);
                synchronizationController.UseMediator(synchronizationMediator);
                roomMusicPlayerController.RoomMusicPlayerModelInitialization(roomObserverThread.RoomMusicPlayerModel!, roomID);
                roomObserverThread.Start();
            }
            return roomObserverThread;
        }

        public IRoomThread? FindRoomObserverThread(string roomID)
        {
            return roomObserversProvider.FindRoomObserverThread(roomID);
        }

        public async Task AddRoomMusic(IClientProxy clients, string roomID, string connectionID, string musicID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.AddRoomMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, musicID);
        }

        public async Task GetRoomMusicList(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetClientsRoomMusicList(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public async Task GetCurrentMusic(IClientProxy client, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetClientMusic(client, roomObserver.RoomMusicPlayerModel);
        }

        public async Task SetCurrentMusic(IClientProxy clients, string roomID, string connectionID, string musicID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetCurrentMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, musicID);
        }

        public async Task SetPlayState(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetPlayState(clients, roomObserver.RoomMusicPlayerModel);
        }

        public async Task SetPauseState(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetPauseState(clients, roomObserver.RoomMusicPlayerModel);
        }

        public async Task SetNextMusic(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetNextMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public async Task SetPreviousMusic(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetPreviousMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public async Task SetAudioTime(IClientProxy clients, string roomID, string connectionID, double value)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetAudioTime(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, value);
        }

        public async Task DeleteRoomMusic(IClientProxy clients, string roomID, string connectionID, string musicID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.DeleteRoomMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, musicID);
        }

        public void CurrentTimePingResponse(string roomID, string connectionID, double value)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                synchronizationController.CurrentTimePingResponse(roomObserver.RoomMusicPlayerModel, connectionID, value);
        }

        public async Task SetPlayNextState(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetPlayNextState(clients, roomObserver.RoomMusicPlayerModel);
        }

        public async Task SetPlayLoopState(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetPlayLoopState(clients, roomObserver.RoomMusicPlayerModel);
        }

        public async Task SetAutoplayNextMusic(IClientProxy clients, string roomID, string musicID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetAutoplayNextMusic(clients, int.Parse(roomID), musicID, roomObserver.RoomMusicPlayerModel);
        }
    }
}