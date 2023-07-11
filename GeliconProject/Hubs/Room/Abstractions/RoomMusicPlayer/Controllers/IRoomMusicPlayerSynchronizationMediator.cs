using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers
{
    public interface IRoomMusicPlayerSynchronizationMediator
    {
        public void SubscribeOnSynchronizationModelEvent(Action<IRoomMusicPlayerModel> action);

        public void InvokeSynchronizationModel(IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SubscribeOnSynchronizationClientEvent(Action<string, IClientProxy, IRoomMusicPlayerModel, int> action);

        public void InvokeSynchronizationClient(string connectionID, IClientProxy client, IRoomMusicPlayerModel roomMusicPlayerModel, int ping);

        public void SubscribeOnSendCurrentTimePingEvent(Action<IClientProxy> action);

        public void InvokeSendCurrentTimePing(IClientProxy client);

        public void SubscribeOnSendPingEvent(Action<IClientProxy> action);

        public void InvokeSendPing(IClientProxy client);

        public void SubscribeOnNextMusicEvent(Action<IClientProxy> action);

        public void InvokeNextMusic(IClientProxy client);
    }
}
