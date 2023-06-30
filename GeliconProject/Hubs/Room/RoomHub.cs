using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.Threads;
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
        private IStorage storage;
        private IRoomsThreadsController roomsThreadsController;

        public RoomHub(IStorage storage, IRoomsThreadsController roomsThreadsController)
        {
            this.storage = storage;
            this.roomsThreadsController = roomsThreadsController;
        }

        public override async Task<Task> OnConnectedAsync()
        {
            await base.OnConnectedAsync();
            await Clients.Caller.SendAsync("Connected");
            return Task.CompletedTask;
        }

        public async Task UserConnect(string roomID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomID);
            roomsThreadsController.CreateRoomObserverThread(roomID, Clients).AddNewClient(Context.ConnectionId, Clients.Client(Context.ConnectionId));
        }

        [Authorize]
        public async Task Send(string message, string roomID)
        {
            Random random = new Random();
            int userID = int.Parse(Context.User!.FindFirst(Claims.UserID)!.Value);
            User? sender = storage.GetRepository<IUserRepository>()?.GetUserByIDWithoutJoins(userID);

            await Clients.Group(roomID).SendAsync("AppendMessage", new
            {
                message,
                sender,
                key = DateTime.Now.Millisecond + random.Next(-10000, 10000),
            });
        }

        [Authorize]
        public async Task PingResponse(string roomID)
        {
            DateTime responseReceived = DateTime.Now;
            IRoomObserverThread? roomObserverThread = roomsThreadsController.FindRoomObserverThread(roomID);

            if (roomObserverThread != null)
                await roomObserverThread.HandlePingResponse(Context.ConnectionId, responseReceived);
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
            await storage.GetRepository<IRoomMusicRepository>()?.Add(int.Parse(roomID), musicID)!;
            storage.Save();
        }

        [Authorize]
        public void SetRoomMusic(string roomID, string musicID)
        {
            roomsThreadsController.SetCurrentMusic(Clients.Group(roomID), roomID, Context.ConnectionId, musicID);
        }

        [Authorize]
        public async Task GetRoomMusicList(string roomID)
        {
            roomsThreadsController.GetRoomMusicList(Clients.Caller, roomID, Context.ConnectionId);
        }

        [Authorize]
        public void GetCurrentMusic(string roomID)
        {
            roomsThreadsController.GetCurrentMusic(Clients.Caller, roomID, Context.ConnectionId);
        }

        [Authorize]
        public void SetPlayState(string roomID)
        {
            roomsThreadsController.SetPlayState(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public void SetPauseState(string roomID)
        {
            roomsThreadsController.SetPauseState(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public void SetNextMusic(string roomID)
        {
            roomsThreadsController.SetNextMusic(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public void SetPreviousMusic(string roomID)
        {
            roomsThreadsController.SetPreviousMusic(Clients.Group(roomID), roomID, Context.ConnectionId);
        }

        [Authorize]
        public void SetAudioTime(string roomID, double value)
        {
            roomsThreadsController.SetAudioTime(Clients.Group(roomID), roomID, Context.ConnectionId, value);
        }

        [Authorize]
        public async Task DeleteRoomMusic(string roomID, string musicID)
        {
            await roomsThreadsController.DeleteRoomMusic(Clients.Group(roomID), roomID, Context.ConnectionId, musicID);
        }
    }
}