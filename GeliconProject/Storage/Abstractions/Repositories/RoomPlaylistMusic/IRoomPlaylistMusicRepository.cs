using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.RoomPlaylistMusic
{
    public interface IRoomPlaylistMusicRepository : IRepository
    {
        public List<Models.RoomMusic>? GetRoomMusicFromPlaylist(int roomPlaylistID, int start, int count);

        public void DeleteRoomMusicFromPlaylist(int roomPlaylistID, string musicID);

        public Task AddRoomPlaylistMusic(int roomPlaylistID, Models.RoomMusic roomMusic);

        public List<Models.RoomPlaylist?>? GetRoomMusicPlaylists(int roomID, string musicID);
    }
}
