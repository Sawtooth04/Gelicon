using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsManager;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsProvider;
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
        private IRoomsThreadsProvider roomsThreadsProvider;
        private IRoomsThreadsManager roomsThreadsManager;
        private IStorage storage;

        public RoomHub(IStorage storage, IRoomsThreadsManager roomsThreadsManager, IRoomsThreadsProvider roomsThreadsProvider)
        {
            this.storage = storage;
            this.roomsThreadsManager = roomsThreadsManager;
            this.roomsThreadsProvider = roomsThreadsProvider;
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
            roomsThreadsProvider.CreateRoomObserverThread(roomID, Clients).AddNewClient(Context.ConnectionId, Clients.Client(Context.ConnectionId));
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
            IRoomObserverThread? roomObserverThread = roomsThreadsProvider.FindRoomObserverThread(roomID);

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
        public async Task GetRoomMusic(string roomID)
        {
            List<RoomMusic>? roomMusics = storage.GetRepository<IRoomMusicRepository>()?.GetRoomMusic(int.Parse(roomID));
            await Clients.Caller.SendAsync("SetRoomMusic", roomMusics);
        }

        [Authorize]
        public void GetCurrentMusic(string roomID)
        {
            roomsThreadsManager.Invoke("GetCurrentMusic", roomID, Context.ConnectionId);
        }
    }
}