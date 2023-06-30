using GeliconProject.Hubs.Room.Abstractions.Threads;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer
{
    public delegate void OnUserJoin(IClientObserverThread clientObserver);

    public interface IRoomMusicPlayerController
    {
        public void RoomMusicPlayerModelInitialization(IRoomMusicPlayerModel roomMusicPlayerModel, string roomID);

        public Task SetClientsRoomMusicList(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SetClientMusic(IClientProxy client, IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SetPlayState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SetPauseState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SetNextMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SetPreviousMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SetAudioTime(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, double value);

        public void SetCurrentMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID);

        public Task DeleteRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID);
    }
}
