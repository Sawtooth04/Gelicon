using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.RoomUser
{
    public interface IRoomUserRepository : IRepository
    {
        public List<Models.User>? GetRoomUsers(int roomID);

        public void AddRoomUser(int roomID, int userID);
    }
}
