using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.RoomUserColor
{
    public interface IRoomUserColorRepository : IRepository
    {
        public void ChangeRoomUserColor(int roomID, int userID, string color);

        public Task<List<Models.RoomUserColor>> GetRoomUsersColor(int roomID);

        public void AddDefaultRoomUserColor(Models.Room room, Models.User user);
    }
}
