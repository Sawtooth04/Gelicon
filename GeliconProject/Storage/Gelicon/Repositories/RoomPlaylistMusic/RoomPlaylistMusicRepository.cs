using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions.Repositories.RoomPlaylistMusic;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.Storage.Gelicon.Repositories.RoomPlaylistMusic
{
    public class RoomPlaylistMusicRepository : IRoomPlaylistMusicRepository
    {
        private IStorageContext storageContext;

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext;
        }

        public List<Models.RoomMusic>? GetRoomMusicFromPlaylist(int roomPlaylistID, int start, int count)
        {
            try
            {
                return storageContext.RoomPlaylistsMusic.Where(r => r.roomPlaylistID == roomPlaylistID).Include(r => r.roomMusic)
                    .Select(r => r.roomMusic).OrderByDescending(r => r.addedAt).Skip(start).Take(count).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void DeleteRoomMusicFromPlaylist(int roomPlaylistID, string musicID)
        {
            try
            {
                storageContext.RoomPlaylistsMusic.Remove(storageContext.RoomPlaylistsMusic.Where(r => r.roomMusic.musicID == musicID &&
                    r.roomPlaylistID == roomPlaylistID).Single());
            }
            catch (Exception){}
        }

        public async Task AddRoomPlaylistMusic(int roomPlaylistID, Models.RoomMusic roomMusic)
        {
            try
            {
                Models.RoomPlaylistMusic? roomPlaylistMusic = storageContext.RoomPlaylistsMusic.Include(r => r.roomMusic).Where(
                    r => r.roomPlaylistID == roomPlaylistID && r.roomMusic!.musicID == roomMusic.musicID).Single();
            }
            catch (Exception)
            {
                await storageContext.RoomPlaylistsMusic.AddAsync(new Models.RoomPlaylistMusic()
                {
                    roomPlaylistID = roomPlaylistID,
                    roomMusic = roomMusic
                });
            }
        }

        public List<Models.RoomPlaylist?>? GetRoomMusicPlaylists(int roomID, string musicID)
        {
            try
            {
                return storageContext.RoomPlaylistsMusic.Include(r => r.roomMusic).Include(r => r.roomPlaylist)
                    .Where(r => r.roomPlaylist!.roomID == roomID && r.roomMusic!.musicID == musicID).Select(r => r.roomPlaylist).ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
