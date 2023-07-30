using GeliconProject.Hubs.Room.Abstractions.Chat;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThread;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsController;
using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.RoomMusic;
using GeliconProject.Storage.Repositories.User;
using GeliconProject.Utils.Audius;
using GeliconProject.Utils.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GeliconProject.Hubs.Room
{
    public class RoomHub : Hub
    {
        private IRoomsThreadsController roomsThreadsController;
        private IRoomChatController roomChatController;

        public RoomHub(IRoomsThreadsController roomsThreadsController, IRoomChatController roomChatController)
        {
            this.roomsThreadsController = roomsThreadsController;
            this.roomChatController = roomChatController;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("Connected");
            return Task.CompletedTask;
        }

        public async Task UserConnect(string roomID)
        {
            IClientProxy client = Clients.Client(Context.ConnectionId);
            int userID = int.Parse(Context.User!.FindFirst(Claims.UserID)!.Value);

            await Groups.AddToGroupAsync(Context.ConnectionId, roomID);
            roomsThreadsController.CreateRoomObserverThread(roomID).AddNewClient(Context.ConnectionId, userID, client);
        }

        [Authorize]
        public async Task SendMessage(string message, string roomID)
        {
            int userID = int.Parse(Context.User!.FindFirst(Claims.UserID)!.Value);
            await roomChatController.SendMessage(message, roomID, userID, Clients.Group(roomID));
        }

        [Authorize]
        public void PingResponse(string roomID)
        {
            DateTime responseReceived = DateTime.Now;
            IRoomThread? roomObserverThread = roomsThreadsController.FindRoomObserverThread(roomID);

            if (roomObserverThread != null)
                roomObserverThread.HandlePingResponse(Context.ConnectionId, responseReceived);
        }

        [Authorize]
        public void CurrentTimePingResponse(string roomID, double value)
        {
            roomsThreadsController.CurrentTimePingResponse(roomID, Context.ConnectionId, value);
        }

        [Authorize]
        public async Task GetMainAudioEndpoint()
        {
            await Clients.Caller.SendAsync("SetMainAudioEndpoint", AudiusParameters.GetServersEndpoint);
        }

        [Authorize]
        public async Task GetAppName()
        {
            await Clients.Caller.SendAsync("SetAppName", AudiusParameters.AppName);
        }

        [Authorize]
        public async Task AddMusicToRoom(string roomID, string musicID)
        {
            await roomsThreadsController.AddRoomMusic(Clients.Group(roomID), roomID, Context.ConnectionId, musicID);
        }

        [Authorize]
        public async Task SetRoomMusic(string roomID, string musicID)
        {
            await roomsThreadsController.SetCurrentMusic(Clients.Group(roomID), roomID, Context.ConnectionId, musicID);
        }

        [Authorize]
        public async Task GetRoomMusicList(string roomID, int start, int count, bool append)
        {
            await roomsThreadsController.GetRoomMusicList(Clients.Caller, roomID, start, count, append);
        }

        [Authorize]
        public async Task GetCurrentMusic(string roomID)
        {
            await roomsThreadsController.GetCurrentMusic(Clients.Caller, roomID, Context.ConnectionId);
        }

        [Authorize]
        public async Task SetPlayState(string roomID)
        {
            await roomsThreadsController.SetPlayState(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public async Task SetPauseState(string roomID)
        {
            await roomsThreadsController.SetPauseState(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public async Task SetNextMusic(string roomID)
        {
            await roomsThreadsController.SetNextMusic(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public async Task SetPreviousMusic(string roomID)
        {
            await roomsThreadsController.SetPreviousMusic(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public async Task SetAudioTime(string roomID, double value)
        {
            await roomsThreadsController.SetAudioTime(Clients.Group(roomID), roomID, Context.ConnectionId, value);
        }

        [Authorize]
        public async Task DeleteRoomMusic(string roomID, string musicID)
        {
            await roomsThreadsController.DeleteRoomMusic(Clients.Group(roomID), roomID, Context.ConnectionId, musicID);
        }

        [Authorize]
        public async Task SetPlayNextState(string roomID)
        {
            await roomsThreadsController.SetPlayNextState(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public async Task SetPlayLoopState(string roomID)
        {
            await roomsThreadsController.SetPlayLoopState(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public async Task SetAutoplayNextMusic(string roomID, string musicID)
        {
            await roomsThreadsController.SetAutoplayNextMusic(Clients.Group(roomID), roomID, musicID, Context.ConnectionId);
        }

        [Authorize]
        public async Task DeleteMessage(int key, string roomID)
        {
            int userID = int.Parse(Context.User!.FindFirst(Claims.UserID)!.Value);
            await roomChatController.DeleteMessage(key, Clients.Group(roomID));
        }

        [Authorize]
        public async Task GetOnlineUsersList(string roomID)
        {
            await roomsThreadsController.GetOnlineUsersList(Clients.Caller, roomID);
        }

        [Authorize]
        public async Task SetRoomUserChanges(string roomID, int userID, string color)
        {
            await roomsThreadsController.SetRoomUserChanges(Clients.Caller, roomID, userID, color);
        }

        [Authorize]
        public async Task GetPlaylists(string roomID)
        {
            await roomsThreadsController.GetPlaylists(Clients.Caller, roomID);
        }

        [Authorize]
        public async Task AddPlaylist(string roomID, string name)
        {
            await roomsThreadsController.AddPlaylist(Clients.Caller, roomID, name);
        }

        [Authorize]
        public async Task DeletePlaylist(string roomID, int roomPlaylistID)
        {
            await roomsThreadsController.DeletePlaylist(Clients.Caller, roomID, roomPlaylistID);
        }
    }
}