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
            await StorageContext.RoomMusics.AddAsync(new Models.RoomMusic() { roomID = roomID, musicID = musicID });
        }

        public List<Models.RoomMusic> GetRoomMusic(int roomID)
        {
            return StorageContext.RoomMusics.Where(r => r.roomID == roomID).ToList();
        }
    }
}
