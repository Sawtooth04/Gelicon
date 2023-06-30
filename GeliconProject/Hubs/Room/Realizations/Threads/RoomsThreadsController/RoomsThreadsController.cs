using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomObserversProvider;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsController;
using GeliconProject.Storage.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads.ThreadsController
{
    public class RoomsThreadsController : IRoomsThreadsController
    {
        private IRoomObserversProvider roomObserversProvider;
        private IRoomMusicPlayerController roomMusicPlayerController;

        public RoomsThreadsController(IRoomObserversProvider roomObserversProvider, IRoomMusicPlayerController roomMusicPlayerController)
        {
            this.roomObserversProvider = roomObserversProvider;
            this.roomMusicPlayerController = roomMusicPlayerController;
        }

        public IRoomObserverThread CreateRoomObserverThread(string roomID, IHubCallerClients clients)
        {
            IRoomObserverThread roomObserverThread = roomObserversProvider.CreateRoomObserverThread(roomID, clients);
            roomMusicPlayerController.RoomMusicPlayerModelInitialization(roomObserverThread.RoomMusicPlayerModel!, roomID);
            return roomObserverThread;
        }

        public IRoomObserverThread? FindRoomObserverThread(string roomID)
        {
            return roomObserversProvider.FindRoomObserverThread(roomID);
        }

        public void GetRoomMusicList(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetClientsRoomMusicList(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public void GetCurrentMusic(IClientProxy client, string roomID, string connectionID)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetClientMusic(client, roomObserver.RoomMusicPlayerModel);
        }

        public void SetCurrentMusic(IClientProxy clients, string roomID, string connectionID, string musicID)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetCurrentMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, musicID);
        }

        public void SetPlayState(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
            {
                roomObserver.RoomMusicPlayerModel.IsPlaying = true;
                roomMusicPlayerController.SetPlayState(clients, roomObserver.RoomMusicPlayerModel);
            }
        }

        public void SetPauseState(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
            {
                roomObserver.RoomMusicPlayerModel.IsPlaying = false;
                roomMusicPlayerController.SetPauseState(clients, roomObserver.RoomMusicPlayerModel);
            }
        }

        public void SetNextMusic(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetNextMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public void SetPreviousMusic(IClientProxy clients, string roomID, string connectionID)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetPreviousMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel);
        }

        public void SetAudioTime(IClientProxy clients, string roomID, string connectionID, double value)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                roomMusicPlayerController.SetAudioTime(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, value);
        }

        public async Task DeleteRoomMusic(IClientProxy clients, string roomID, string connectionID, string musicID)
        {
            IRoomObserverThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.DeleteRoomMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel, musicID);
        }
    }
}
