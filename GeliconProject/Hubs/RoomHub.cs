using GeliconProject.Models;
using GeliconProject.Repositories;
using GeliconProject.Utils.ApplicationContexts;
using GeliconProject.Utils.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GeliconProject.Hubs
{
    public class RoomHub : Hub
    {
        private IRepository repository;

        public RoomHub(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task Init(string roomID)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomID);
        }

        [Authorize]
        public async Task Send(string message, string roomID)
        {
            Random random = new Random();
            int userID = int.Parse(Context.User!.FindFirst(Claims.UserID)!.Value);
            User? sender = repository.GetUserByIDWithoutJoins(userID);

            await Clients.Group(roomID).SendAsync("AppendMessage", new
            {
                message = message,
                sender = sender,
                key = DateTime.Now.Millisecond + random.Next(-10000, 10000),
            });
        }
    }
}
