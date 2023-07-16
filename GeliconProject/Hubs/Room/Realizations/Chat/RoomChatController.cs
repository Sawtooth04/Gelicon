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

        public async Task SendMessage(string message, string roomID, int userID, IClientProxy clients)
        {
            Random random = new Random();
            User? sender = storage.GetRepository<IUserRepository>()?.GetUserByIDWithoutJoins(userID);

            await clients.SendAsync("AppendMessage", new
            {
                message,
                sender,
                key = DateTime.Now.Millisecond + random.Next(-10000, 10000),
            });
        }

        public async Task DeleteMessage(int key, IClientProxy clients)
        {
            await clients.SendAsync("DeleteMessage", key);
        }
    }
}
