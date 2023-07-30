using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.RoomPlaylist
{
    public interface IRoomPlaylistRepository : IRepository
    {
        public List<Models.RoomPlaylist> GetRoomPlaylists(int roomID);

        public Task AddRoomPlaylist(int roomID, string name);

        public void DeleteRoomPlaylist(int roomPlaylistID);

        public void ChangeRoomPlaylist(int roomID, int roomPlaylistID, string name);
    }
}
