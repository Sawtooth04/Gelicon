using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions.Repositories.RoomMusic;

namespace GeliconProject.Storage.Gelicon.Repositories.RoomMusic
{
    public class RoomMusicRepository : IRoomMusicRepository
    {
        private IStorageContext StorageContext { get; set; }

        public void SetStorageContext(IStorageContext storageContext)
        {
            StorageContext = storageContext;
        }

        public async Task Add(int roomID, string musicID)
        {
            await StorageContext.RoomMusics.AddAsync(new Models.RoomMusic() { roomID = roomID, musicID = musicID, addedAt = DateTime.UtcNow});
        }

        public async Task<List<Models.RoomMusic>> GetRoomMusic(int roomID)
        {
            return await Task.Run(() => StorageContext.RoomMusics.Where(r => r.roomID == roomID).ToList());
        }

        public Models.RoomMusic? GetRoomMusic(int roomID, string musicID)
        {
            try
            {
                return StorageContext.RoomMusics.Where(r => r.musicID == musicID && r.roomID == roomID).Single();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Models.RoomMusic GetLatestAddedMusic(int roomID, bool isDescending = true)
        {
            DateTime latestAddedMusicTime = ((isDescending) ?
                StorageContext.RoomMusics.Where(r => r.roomID == roomID).Max(r => r.addedAt) :
                StorageContext.RoomMusics.Where(r => r.roomID == roomID).Min(r => r.addedAt));

            try
            {
                return StorageContext.RoomMusics.Where(r => r.roomID == roomID && r.addedAt == latestAddedMusicTime).Single();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Models.RoomMusic GetNextMusic(int roomID, Models.RoomMusic currentMusic, bool isDescending = true)
        {
            Models.RoomMusic result;

            try
            {
                if (isDescending)
                    result = StorageContext.RoomMusics.Where(r => r.roomID == roomID && r.addedAt < currentMusic.addedAt)
                        .OrderByDescending(r => r.addedAt).First();
                else
                    result = StorageContext.RoomMusics.Where(r => r.roomID == roomID && r.addedAt > currentMusic.addedAt)
                        .OrderBy(r => r.addedAt).First();
                return result;
            }
            catch (Exception)
            {
                return (isDescending) ? StorageContext.RoomMusics.OrderByDescending(r => r.addedAt).First() :
                    StorageContext.RoomMusics.OrderByDescending(r => r.addedAt).Last();
            }
        }

        public Models.RoomMusic GetPreviousMusic(int roomID, Models.RoomMusic currentMusic, bool isDescending = true)
        {
            Models.RoomMusic result;

            try
            {
                if (isDescending)
                    result = StorageContext.RoomMusics.Where(r => r.roomID == roomID && r.addedAt > currentMusic.addedAt)
                        .OrderByDescending(r => r.addedAt).Last();
                else
                    result = StorageContext.RoomMusics.Where(r => r.roomID == roomID && r.addedAt < currentMusic.addedAt)
                        .OrderBy(r => r.addedAt).Last();
                return result;
            }
            catch (Exception)
            {
                return (isDescending) ? StorageContext.RoomMusics.OrderByDescending(r => r.addedAt).Last() :
                    StorageContext.RoomMusics.OrderByDescending(r => r.addedAt).First();
            }
        }

        public void DeleteRoomMusic(int roomID, string musicID)
        {
            try
            {
                StorageContext.RoomMusics.Remove(StorageContext.RoomMusics.Where(r => r.musicID == musicID && r.roomID == roomID).Single());
                StorageContext.SaveChanges();
            }
            catch (Exception e)
            {
            }
        }
    }
}
