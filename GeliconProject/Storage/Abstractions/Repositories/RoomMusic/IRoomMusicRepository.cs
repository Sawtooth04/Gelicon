using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.RoomMusic
{
    public interface IRoomMusicRepository : IRepository
    {
        public Task Add(int roomID, string musicID);

        public List<Models.RoomMusic> GetRoomMusic(int roomID);

        public Models.RoomMusic GetLatestAddedMusic(int roomID, bool isDescending = true);

        public Models.RoomMusic GetNextMusic(int roomID, Models.RoomMusic currentMusic, bool isDescending = true);

        public Models.RoomMusic GetPreviousMusic(int roomID, Models.RoomMusic currentMusic, bool isDescending = true);
    }
}
