using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions.Repositories.RoomPlaylist;

namespace GeliconProject.Storage.Gelicon.Repositories.RoomPlaylist
{
    public class RoomPlaylistRepository : IRoomPlaylistRepository
    {
        private IStorageContext storageContext;

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext;
        }

        public List<Models.RoomPlaylist> GetRoomPlaylists(int roomID)
        {
            try
            {
                return storageContext.RoomPlaylists.Where(r => r.roomID == roomID).ToList();
            }
            catch (Exception)
            {
                return new List<Models.RoomPlaylist>();
            }
        }

        public async Task AddRoomPlaylist(int roomID, string name)
        {
            Models.RoomPlaylist playlist = new Models.RoomPlaylist()
            {
                roomID = roomID,
                addedAt = DateTime.UtcNow,
                name = name
            };
            await storageContext.RoomPlaylists.AddAsync(playlist);
        }

        public void DeleteRoomPlaylist(int roomPlaylistID)
        {
            storageContext.RoomPlaylists.Remove(storageContext.RoomPlaylists.Where(r => r.roomPlaylistID == roomPlaylistID).Single());
        }

        public void ChangeRoomPlaylist(int roomID, int roomPlaylistID, string name)
        {
            Models.RoomPlaylist roomPlaylist = storageContext.RoomPlaylists.Where(r => r.roomPlaylistID == roomPlaylistID &&
                r.roomID == roomID).Single();

            if (roomPlaylist != null)
                roomPlaylist.name = name;
        }
    }
}
