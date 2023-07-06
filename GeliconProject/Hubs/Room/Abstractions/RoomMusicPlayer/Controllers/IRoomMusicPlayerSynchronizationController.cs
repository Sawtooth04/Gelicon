using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers
{
    public interface IRoomMusicPlayerSynchronizationController
    {
        public void CurrentTimePingResponse(IRoomMusicPlayerModel roomMusicPlayerModel, string connectionID, double value);

        public void SynchronizeModels(IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SynchronizeClient(string connectionID, IClientProxy client, IRoomMusicPlayerModel roomMusicPlayerModel, int ping);

        public void UseMediator(IRoomMusicPlayerSynchronizationMediator synchronizationMediator);
    }
}
