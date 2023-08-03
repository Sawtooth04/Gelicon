using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThread;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThreadsProvider;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsController;
using GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Controllers;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.RoomPlaylist;
using GeliconProject.Storage.Abstractions.Repositories.RoomUserColor;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GeliconProject.Hubs.Room.Realizations.Threads.ThreadsController
{
    public class RoomsThreadsController : IRoomsThreadsController
    {
        private IRoomThreadsProvider roomObserversProvider;
        private IRoomMusicPlayerController roomMusicPlayerController;
        private IRoomMusicPlayerSynchronizationController synchronizationController;
        private IStorage storage;

        public RoomsThreadsController(IRoomThreadsProvider roomObserversProvider, IRoomMusicPlayerController roomMusicPlayerController,
            IRoomMusicPlayerSynchronizationController synchronizationController, IStorage storage)
        {
            this.roomObserversProvider = roomObserversProvider;
            this.roomMusicPlayerController = roomMusicPlayerController;
            this.synchronizationController = synchronizationController;
            this.storage = storage;
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

        public async Task GetRoomMusicList(IClientProxy clients, string roomID, int start, int count, bool append)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetClientsRoomMusicList(clients, int.Parse(roomID), start, count, append);
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

        public async Task GetOnlineUsersList(IClientProxy client, string roomID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null)
                await client.SendAsync("SetOnlineUsers", roomObserver.GetOnlineUsersID());
        }

        public async Task SetRoomUserChanges(IClientProxy clients, string roomID, int userID, string color)
        {
            IRoomUserColorRepository repository = storage.GetRepository<IRoomUserColorRepository>()!;
            List<Models.RoomUserColor> userColors;

            repository.ChangeRoomUserColor(int.Parse(roomID), userID, color);
            storage.Save();
            userColors = await repository.GetRoomUsersColor(int.Parse(roomID));
            userColors = userColors.ConvertAll(u =>
            {
                u.room = null;
                u.user = null;
                return u;
            });
            await clients.SendAsync("SetUsersColors", userColors);
        }

        public async Task GetPlaylists(IClientProxy clients, string roomID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetClientsPlaylists(clients, int.Parse(roomID));
        }

        public async Task AddPlaylist(IClientProxy clients, string roomID, string name)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.AddPlaylist(clients, int.Parse(roomID), name);
        }

        public async Task DeletePlaylist(IClientProxy clients, string roomID, int roomPlaylistID)
        {
            await roomMusicPlayerController.DeletePlaylist(clients, int.Parse(roomID), roomPlaylistID);
        }

        public async Task SetRoomPlaylistChanges(IClientProxy clients, string roomID, int roomPlaylistID, string name)
        {
            IRoomPlaylistRepository repository = storage.GetRepository<IRoomPlaylistRepository>()!;

            repository.ChangeRoomPlaylist(int.Parse(roomID), roomPlaylistID, name);
            storage.Save();
            await GetPlaylists(clients, roomID);
        }

        public async Task GetPlaylistMusicList(IClientProxy clients, string roomID, int roomPlaylistID, int start, int count, bool append)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetClientsPlaylistMusicList(clients, int.Parse(roomID), roomPlaylistID, start, count, append);
        }

        public void DeletePlaylistMusic(IClientProxy clients, int roomPlaylistID, string musicID)
        {
            roomMusicPlayerController.DeletePlaylistMusic(clients, roomPlaylistID, musicID);
        }

        public async Task AddPlaylistMusic(IClientProxy clients, string roomID, int roomPlaylistID, string musicID)
        {
            await roomMusicPlayerController.AddPlaylistMusic(clients, roomID, roomPlaylistID, musicID);
        }

        public async Task GetRoomMusicPlaylists(IClientProxy clients, string roomID, string musicID)
        {
            await roomMusicPlayerController.GetRoomMusicPlaylists(clients, roomID, musicID);
        }

        public async Task SetCurrentPlaylistRoomMusic(IClientProxy clients, string roomID, int roomPlaylistID, string musicID)
        {
            IRoomThread? roomObserver = roomObserversProvider.FindRoomObserverThread(roomID);

            if (roomObserver != null && roomObserver.RoomMusicPlayerModel != null)
                await roomMusicPlayerController.SetCurrentPlaylistRoomMusic(clients, int.Parse(roomID), roomObserver.RoomMusicPlayerModel,
                    roomPlaylistID, musicID);
        }
    }
}