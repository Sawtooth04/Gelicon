using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.RoomMusic;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Controllers
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

        public async Task SetClientsRoomMusicList(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            List<RoomMusic>? roomMusic = storage.GetRepository<IRoomMusicRepository>()?.GetRoomMusic(roomID);
            await clients.SendAsync("SetRoomMusicList", roomMusic);
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
            clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
        }

        public void SetPreviousMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.CurrentMusic = roomMusicPlayerMusicProvider.GetPreviousMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomID, roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
            );
            clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
        }

        public void SetAudioTime(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, double value)
        {
            clients.SendAsync("SetAudioTime", value);
        }

        public void SetCurrentMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID)
        {
            RoomMusic? roomMusic = roomMusicPlayerMusicProvider.GetGetMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomID, musicID);

            if (roomMusic != null)
            {
                roomMusicPlayerModel.CurrentMusic = roomMusic;
                clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
            }
        }

        public async Task DeleteRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID)
        {
            roomMusicPlayerMusicProvider.GetDeleteMusicAction(roomMusicPlayerModel.Source).Invoke(storage, roomID, musicID);
            await SetClientsRoomMusicList(clients, roomID, roomMusicPlayerModel);
        }
    }
}
