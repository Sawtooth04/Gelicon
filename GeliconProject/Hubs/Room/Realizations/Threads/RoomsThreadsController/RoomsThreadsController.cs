using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.Threads;
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

        public void GetRoomMusicList(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetClientsRoomMusicList(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public void GetCurrentMusic(IClientProxy client, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetClientMusic(client, roomObserver.RoomMusicPlayerModel);
        }

        public void SetCurrentMusic(IClientProxy clients, string roomID, string connectionID, string musicID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetCurrentMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, musicID);
        }

        public void SetPlayState(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
            {
                roomObserver.RoomMusicPlayerModel.IsPlaying = true;
                roomMusicPlayerController.SetPlayState(clients, roomObserver.RoomMusicPlayerModel);
            }
        }

        public void SetPauseState(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
            {
                roomObserver.RoomMusicPlayerModel.IsPlaying = false;
                roomMusicPlayerController.SetPauseState(clients, roomObserver.RoomMusicPlayerModel);
            }
        }

        public void SetNextMusic(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetNextMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public void SetPreviousMusic(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetPreviousMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public void SetAudioTime(IClientProxy clients, string roomID, string connectionID, double value)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetAudioTime(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, value);
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
    }
}