using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.RoomMusic;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer
{
    public class RoomMusicPlayerController : IRoomMusicPlayerController
    {
        private IStorage storage;
        private IRoomMusicPlayerMusicProvider roomMusicPlayerMusicProvider;

        public RoomMusicPlayerController(IStorage storage, IRoomMusicPlayerMusicProvider roomMusicPlayerMusicProvider)
        {
            this.storage = storage;
            this.roomMusicPlayerMusicProvider = roomMusicPlayerMusicProvider;
        }

        public void RoomMusicPlayerModelInitialization(IRoomMusicPlayerModel roomMusicPlayerModel, string roomID)
        {
            roomMusicPlayerModel.CurrentMusic = storage.GetRepository<IRoomMusicRepository>().GetLatestAddedMusic(int.Parse(roomID));
        }

        public async void SetClientMusic(IClientProxy client, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            await client.SendAsync("SetMusic", roomMusicPlayerModel.CurrentMusic);
        }

        public void SetPlayState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.IsPlaying = true;
            clients.SendAsync("SetPlayState");
        }

        public void SetPauseState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.IsPlaying = false;
            clients.SendAsync("SetPauseState");
        }

        public void SetNextMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.CurrentMusic = roomMusicPlayerMusicProvider.GetNextMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomID, roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
            );
            clients.SendAsync("SetMusic", roomMusicPlayerModel.CurrentMusic);
        }

        public void SetPreviousMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.CurrentMusic = roomMusicPlayerMusicProvider.GetPreviousMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomID, roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
            );
            clients.SendAsync("SetMusic", roomMusicPlayerModel.CurrentMusic);
        }
    }
}
