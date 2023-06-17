using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.RoomMusic
{
    public interface IRoomMusicRepository : IRepository
    {
        public Task Add(int roomID, string musicID);

        public List<Models.RoomMusic> GetRoomMusic(int roomID);
    }
}
