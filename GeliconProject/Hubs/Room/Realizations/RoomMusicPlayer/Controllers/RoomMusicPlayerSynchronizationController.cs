using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Controllers
{
    public class RoomMusicPlayerSynchronizationController : IRoomMusicPlayerSynchronizationController
    {
        private IRoomMusicPlayerSynchronizationModel synchronizationModel;

        public RoomMusicPlayerSynchronizationController(IRoomMusicPlayerSynchronizationModel synchronizationModel)
        {
            this.synchronizationModel = synchronizationModel;
        }

        public async void SendCurrentTimePing(IClientProxy client)
        {
            await client.SendAsync("CurrentTimePingReceive");
        }

        public async void SendPing(IClientProxy client)
        {
            await client.SendAsync("PingReceive");
        }

        public void CurrentTimePingResponse(IRoomMusicPlayerModel roomMusicPlayerModel, string connectionID, double value)
        {
            roomMusicPlayerModel.SetClientCurrentTime(connectionID, value);
        }

        public void SynchronizeModels(IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            synchronizationModel.SynchronizeModels(roomMusicPlayerModel);
        }

        public void SynchronizeClient(string connectionID, IClientProxy client, IRoomMusicPlayerModel roomMusicPlayerModel, int ping)
        {
            IClientRoomMusicPlayerModel? clientRoomMusicPlayerModel = roomMusicPlayerModel.GetClientModel(connectionID);

            if (clientRoomMusicPlayerModel != null)
            {
                if (clientRoomMusicPlayerModel.NeedToRewind)
                    client.SendAsync("SetAudioTime", synchronizationModel.GetCurrentTime(roomMusicPlayerModel) + ping / 500);
                else
                    client.SendAsync("SetSpeedRatio", clientRoomMusicPlayerModel.SpeedRatio);
            }
        }

        public void UseMediator(IRoomMusicPlayerSynchronizationMediator synchronizationMediator)
        {
            synchronizationMediator.SubscribeOnSynchronizationModelEvent(SynchronizeModels);
            synchronizationMediator.SubscribeOnSynchronizationClientEvent(SynchronizeClient);
            synchronizationMediator.SubscribeOnSendCurrentTimePingEvent(SendCurrentTimePing);
            synchronizationMediator.SubscribeOnSendPingEvent(SendPing);
        }
    }
}
