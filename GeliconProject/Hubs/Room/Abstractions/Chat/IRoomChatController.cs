using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Chat
{
    public interface IRoomChatController
    {
        public Task SendMessage(string message, string roomID, int userID, IClientProxy clients);

        public Task DeleteMessage(int key, IClientProxy clients);
    }
}
