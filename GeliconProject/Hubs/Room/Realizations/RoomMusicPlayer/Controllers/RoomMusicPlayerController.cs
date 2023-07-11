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

        public async Task AddRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID)
        {
            await roomMusicPlayerMusicProvider.GetAddMusicAction(roomMusicPlayerModel.Source).Invoke(storage, roomID, musicID);
            await SetClientsRoomMusicList(clients, roomID, roomMusicPlayerModel);
        }

        public async Task SetClientsRoomMusicList(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            List<RoomMusic> roomMusic = await storage.GetRepository<IRoomMusicRepository>().GetRoomMusic(roomID);
            await clients.SendAsync("SetRoomMusicList", roomMusic);
        }

        public async Task SetClientMusic(IClientProxy client, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            await client.SendAsync("SetMusic", roomMusicPlayerModel.CurrentMusic);
        }

        public async Task SetPlayState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.IsPlaying = true;
            await clients.SendAsync("SetPlayState");
        }

        public async Task SetPauseState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.IsPlaying = false;
            await clients.SendAsync("SetPauseState");
        }

        public async Task SetNextMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.CurrentMusic = roomMusicPlayerMusicProvider.GetNextMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomID, roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
            );
            await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
        }

        public async Task SetPreviousMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.CurrentMusic = roomMusicPlayerMusicProvider.GetPreviousMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomID, roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
            );
            await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
        }

        public async Task SetAudioTime(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, double value)
        {
            await clients.SendAsync("SetAudioTime", value);
        }

        public async Task SetCurrentMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID)
        {
            RoomMusic? roomMusic = roomMusicPlayerMusicProvider.GetGetMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomID, musicID);

            if (roomMusic != null)
            {
                roomMusicPlayerModel.CurrentMusic = roomMusic;
                await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
            }
        }

        public async Task DeleteRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID)
        {
            roomMusicPlayerMusicProvider.GetDeleteMusicAction(roomMusicPlayerModel.Source).Invoke(storage, roomID, musicID);
            await SetClientsRoomMusicList(clients, roomID, roomMusicPlayerModel);
        }

        public async Task SetPlayNextState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.PlayNext = true;
            await clients.SendAsync("SetPlayNextState");
        }

        public async Task SetPlayLoopState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.PlayNext = false;
            await clients.SendAsync("SetPlayLoopState");
        }

        public async Task SetAutoplayNextMusic(IClientProxy clients, int roomID, string musicID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            if (roomMusicPlayerModel.PlayNext)
                await AutoPlayNext(clients, roomID, musicID, roomMusicPlayerModel);
            else
            {
                await SetAudioTime(clients, roomID, roomMusicPlayerModel, 0);
                await SetPlayState(clients, roomMusicPlayerModel);
            }
        }

        private async Task AutoPlayNext(IClientProxy clients, int roomID, string musicID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            RoomMusic? currentMusic = storage.GetRepository<IRoomMusicRepository>().GetRoomMusic(roomID, musicID);
            RoomMusic nextMusic;

            if (currentMusic != null)
            {
                nextMusic = roomMusicPlayerMusicProvider.GetNextMusicAction(roomMusicPlayerModel.Source).Invoke(
                    storage, roomID, roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
                );
                if (currentMusic.musicID != nextMusic.musicID)
                {
                    roomMusicPlayerModel.CurrentMusic = nextMusic;
                    await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
                }
            }
        }
    }
}
