using GeliconProject.Hubs.Room.Threads;
using GeliconProject.Hubs.Room.Threads.ThreadsContainer;
using GeliconProject.Models;
using GeliconProject.Repositories;
using GeliconProject.Utils.ApplicationContexts;
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
        private IRoomsThreadsContainer roomsThreadsContainer;
        private IRepository repository;

        public RoomHub(IRepository repository, IRoomsThreadsContainer roomsThreadsContainer)
        {
            this.repository = repository;
            this.roomsThreadsContainer = roomsThreadsContainer;
        }

        public async Task Init(string roomID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomID);
            roomsThreadsContainer.CreateRoomObserverThread(roomID, Clients).AddNewClient(Context.ConnectionId);
        }

        [Authorize]
        public async Task Send(string message, string roomID)
        {
            Random random = new Random();
            int userID = int.Parse(Context.User!.FindFirst(Claims.UserID)!.Value);
            User? sender = repository.GetUserByIDWithoutJoins(userID);

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
            RoomObserverThread? roomObserverThread = roomsThreadsContainer.FindRoomObserverThread(roomID);

            if (roomObserverThread != null)
                await roomObserverThread.HandlePingResponse(Context.ConnectionId, responseReceived);
        }
    }
}
