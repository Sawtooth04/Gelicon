using GeliconProject.Hubs.Room.Abstractions.Chat;
using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Repositories.User;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Chat
{
    public class RoomChatController : IRoomChatController
    {
        private IStorage storage;

        public RoomChatController(IStorage storage)
        {
            this.storage = storage;
        }

        public async Task SendAsync(string message, string roomID, int userID, IHubCallerClients clients)
        {
            Random random = new Random();
            User? sender = storage.GetRepository<IUserRepository>()?.GetUserByIDWithoutJoins(userID);

            await clients.Group(roomID).SendAsync("AppendMessage", new
            {
                message,
                sender,
                key = DateTime.Now.Millisecond + random.Next(-10000, 10000),
            });
        }
    }
}
