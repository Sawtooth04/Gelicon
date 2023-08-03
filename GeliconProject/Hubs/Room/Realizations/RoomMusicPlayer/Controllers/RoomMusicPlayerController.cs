using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.RoomMusic;
using GeliconProject.Storage.Abstractions.Repositories.RoomPlaylist;
using GeliconProject.Storage.Abstractions.Repositories.RoomPlaylistMusic;
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

        //TODO
        public async Task AddRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID)
        {
            await roomMusicPlayerMusicProvider.GetAddMusicAction(roomMusicPlayerModel.Source).Invoke(storage, roomID, musicID);
            //await SetClientsRoomMusicList(clients, roomID, roomMusicPlayerModel);
        }

        public async Task SetClientsRoomMusicList(IClientProxy clients, int roomID, int start, int count, bool append)
        {
            List<RoomMusic> roomMusic = await storage.GetRepository<IRoomMusicRepository>().GetRoomMusic(roomID, start, count);

            if (append)
                await clients.SendAsync("AppendRoomMusicList", roomMusic);
            else
                await clients.SendAsync("AppendBeforeRoomMusicList", roomMusic);
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
                storage, roomMusicPlayerModel.Source == RoomMusicPlayerSource.Music ? roomID : roomMusicPlayerModel.currentPlaylist!.roomPlaylistID,
                roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
            );
            roomMusicPlayerModel.CurrentMusic.roomPlaylistsMusic = null;
            await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
        }

        public async Task SetPreviousMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            roomMusicPlayerModel.CurrentMusic = roomMusicPlayerMusicProvider.GetPreviousMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomMusicPlayerModel.Source == RoomMusicPlayerSource.Music ? roomID : roomMusicPlayerModel.currentPlaylist!.roomPlaylistID,
                roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
            );
            roomMusicPlayerModel.CurrentMusic.roomPlaylistsMusic = null;
            await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
        }

        public async Task SetAudioTime(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, double value)
        {
            await clients.SendAsync("SetAudioTime", value);
        }

        public async Task SetCurrentMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID)
        {
            roomMusicPlayerModel.Source = RoomMusicPlayerSource.Music;
            roomMusicPlayerModel.currentPlaylist = null;
            RoomMusic? roomMusic = roomMusicPlayerMusicProvider.GetGetMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomID, musicID);

            if (roomMusic != null)
            {
                roomMusicPlayerModel.CurrentMusic = roomMusic;
                await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
            }
        }

        //TODO
        public async Task DeleteRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID)
        {
            roomMusicPlayerMusicProvider.GetDeleteMusicAction(roomMusicPlayerModel.Source).Invoke(storage, roomID, musicID);
            //await SetClientsRoomMusicList(clients, roomID, roomMusicPlayerModel);
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
                    storage, roomMusicPlayerModel.Source == RoomMusicPlayerSource.Music ? roomID : roomMusicPlayerModel.currentPlaylist!.roomPlaylistID,
                    roomMusicPlayerModel.CurrentMusic, roomMusicPlayerModel.IsDescending
                );
                if (currentMusic.musicID != nextMusic.musicID)
                {
                    roomMusicPlayerModel.CurrentMusic = nextMusic;
                    nextMusic.roomPlaylistsMusic = null;
                    await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
                }
            }
        }

        public async Task SetClientsPlaylists(IClientProxy clients, int roomID)
        {
            List<RoomPlaylist> roomPlaylists = storage.GetRepository<IRoomPlaylistRepository>().GetRoomPlaylists(roomID);
            await clients.SendAsync("SetRoomPlaylists", roomPlaylists);
        }

        public async Task AddPlaylist(IClientProxy clients, int roomID, string name)
        {
            await storage.GetRepository<IRoomPlaylistRepository>().AddRoomPlaylist(roomID, name);
            storage.Save();
            await SetClientsPlaylists(clients, roomID);
        }

        public async Task DeletePlaylist(IClientProxy clients, int roomID, int roomPlaylistID)
        {
            storage.GetRepository<IRoomPlaylistRepository>().DeleteRoomPlaylist(roomPlaylistID);
            storage.Save();
            await SetClientsPlaylists(clients, roomID);
        }

        public async Task SetClientsPlaylistMusicList(IClientProxy clients, int roomID, int roomPlaylistID, int start, int count, bool append)
        {
            List<RoomMusic>? roomMusic = storage.GetRepository<IRoomPlaylistMusicRepository>().GetRoomMusicFromPlaylist(roomPlaylistID, start, count);

            if (append)
                await clients.SendAsync("AppendPlaylistMusicList", roomMusic);
            else
                await clients.SendAsync("AppendBeforePlaylistMusicList", roomMusic);
        }

        public void DeletePlaylistMusic(IClientProxy clients, int roomPlaylistID, string musicID)
        {
            storage.GetRepository<IRoomPlaylistMusicRepository>().DeleteRoomMusicFromPlaylist(roomPlaylistID, musicID);
            storage.Save();
        }

        public async Task AddPlaylistMusic(IClientProxy clients, string roomID, int roomPlaylistID, string musicID)
        {
            RoomMusic? roomMusic = storage.GetRepository<IRoomMusicRepository>().GetRoomMusic(int.Parse(roomID), musicID);

            if (roomMusic != null)
            {
                await storage.GetRepository<IRoomPlaylistMusicRepository>().AddRoomPlaylistMusic(roomPlaylistID, roomMusic);
                storage.Save();
            }
        }

        public async Task GetRoomMusicPlaylists(IClientProxy clients, string roomID, string musicID)
        {
            await clients.SendAsync("SetRoomMusicPlaylists", 
                storage.GetRepository<IRoomPlaylistMusicRepository>().GetRoomMusicPlaylists(int.Parse(roomID), musicID));
        }

        public async Task SetCurrentPlaylistRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel,
            int roomPlaylistID, string musicID)
        {
            roomMusicPlayerModel.Source = RoomMusicPlayerSource.Playlist;
            roomMusicPlayerModel.currentPlaylist = storage.GetRepository<IRoomPlaylistRepository>().GetRoomPlaylist(roomPlaylistID);
            RoomMusic? roomMusic = roomMusicPlayerMusicProvider.GetGetMusicAction(roomMusicPlayerModel.Source).Invoke(
                storage, roomPlaylistID, musicID);

            if (roomMusic != null)
            {
                roomMusicPlayerModel.CurrentMusic = roomMusic;
                roomMusic.roomPlaylistsMusic = null;
                await clients.SendAsync("SetMusicAfterInit", roomMusicPlayerModel.CurrentMusic);
            }
        }
    }
}
