using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Chat
{
    public interface IRoomChatController
    {
        public Task SendAsync(string message, string roomID, int userID, IHubCallerClients clients);
    }
}
