using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Hubs.Room.Abstractions.Threads.ClientThread;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers
{
    public delegate void OnUserJoin(IClientThread clientObserver);

    public interface IRoomMusicPlayerController
    {
        public void RoomMusicPlayerModelInitialization(IRoomMusicPlayerModel roomMusicPlayerModel, string roomID);

        public Task AddRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID);

        public Task SetClientsRoomMusicList(IClientProxy clients, int roomID, int start, int count, bool append);

        public Task SetClientMusic(IClientProxy client, IRoomMusicPlayerModel roomMusicPlayerModel);

        public Task SetPlayState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel);

        public Task SetPauseState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel);

        public Task SetNextMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel);

        public Task SetPreviousMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel);

        public Task SetAudioTime(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, double value);

        public Task SetCurrentMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID);

        public Task DeleteRoomMusic(IClientProxy clients, int roomID, IRoomMusicPlayerModel roomMusicPlayerModel, string musicID);

        public Task SetPlayNextState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel);

        public Task SetPlayLoopState(IClientProxy clients, IRoomMusicPlayerModel roomMusicPlayerModel);

        public Task SetAutoplayNextMusic(IClientProxy clients, int roomID, string musicID, IRoomMusicPlayerModel roomMusicPlayerModel);

        public Task SetClientsPlaylists(IClientProxy clients, int roomID);

        public Task AddPlaylist(IClientProxy clients, int roomID, string name);

        public Task DeletePlaylist(IClientProxy clients, int roomID, int roomPlaylistID);

        public Task SetClientsPlaylistMusicList(IClientProxy clients, int roomID, int roomPlaylistID, int start, int count, bool append);

        public void DeletePlaylistMusic(IClientProxy clients, int roomPlaylistID, string musicID);

        public Task AddPlaylistMusic(IClientProxy clients, string roomID, int roomPlaylistID, string musicID);

        public Task GetRoomMusicPlaylists(IClientProxy clients, string roomID, string musicID);
    }
}
