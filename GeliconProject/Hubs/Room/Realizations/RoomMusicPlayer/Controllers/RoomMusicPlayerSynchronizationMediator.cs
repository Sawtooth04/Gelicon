using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Controllers
{
    public class RoomMusicPlayerSynchronizationMediator : IRoomMusicPlayerSynchronizationMediator
    {
        private event Action<IRoomMusicPlayerModel>? synchronizationModelEvent;
        private event Action<string, IClientProxy, IRoomMusicPlayerModel, int>? synchronizationClientEvent;
        private event Action<IClientProxy>? sendCurrentTimePingEvent;
        private event Action<IClientProxy>? sendPingEvent;

        public void SubscribeOnSynchronizationModelEvent(Action<IRoomMusicPlayerModel> action)
        {
            synchronizationModelEvent += action;
        }

        public void InvokeSynchronizationModel(IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            synchronizationModelEvent?.Invoke(roomMusicPlayerModel);
        }

        public void SubscribeOnSynchronizationClientEvent(Action<string, IClientProxy, IRoomMusicPlayerModel, int> action)
        {
            synchronizationClientEvent += action;
        }

        public void InvokeSynchronizationClient(string connectionID, IClientProxy client, IRoomMusicPlayerModel roomMusicPlayerModel, int ping)
        {
            synchronizationClientEvent?.Invoke(connectionID, client, roomMusicPlayerModel, ping);
        }

        public void SubscribeOnSendCurrentTimePingEvent(Action<IClientProxy> action)
        {
            sendCurrentTimePingEvent += action;
        }

        public void InvokeSendCurrentTimePing(IClientProxy client)
        {
            sendCurrentTimePingEvent?.Invoke(client);
        }

        public void SubscribeOnSendPingEvent(Action<IClientProxy> action)
        {
            sendPingEvent += action;
        }

        public void InvokeSendPing(IClientProxy client) 
        { 
            sendPingEvent?.Invoke(client);
        }
    }
}
