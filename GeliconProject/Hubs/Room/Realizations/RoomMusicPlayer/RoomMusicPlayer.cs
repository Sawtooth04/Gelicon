using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Model;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer
{
    public class RoomMusicPlayer : IRoomMusicPlayer
    {
        private List<IClientObserverThread> clientObservers;
        private IRoomMusicPlayerModel roomMusicPlayerModel;

        public RoomMusicPlayer(List<IClientObserverThread> clientObservers, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            this.clientObservers = clientObservers;
            this.roomMusicPlayerModel = roomMusicPlayerModel;
        }

        public async void SetClientMusic(IClientObserverThread clientObserver)
        {
            await clientObserver.SendAsync("SetMusic", roomMusicPlayerModel.CurrentMusic);
        }
    }
}
