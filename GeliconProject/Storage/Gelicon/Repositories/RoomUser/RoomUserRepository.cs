using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions.Repositories.RoomUser;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.Storage.Gelicon.Repositories.RoomUser
{
    public class RoomUserRepository : IRoomUserRepository
    {
        private IStorageContext StorageContext { get; set; }

        public void SetStorageContext(IStorageContext storageContext)
        {
            StorageContext = storageContext;
        }

        public List<Models.User>? GetRoomUsers(int roomID)
        {
            return StorageContext.Rooms.Where(r => r.roomID == roomID).Include(r => r.users).First().users;
        }

        public void AddRoomUser(int roomID, int userID)
        {
            List<Models.User>? roomUsers = GetRoomUsers(roomID);
            if (roomUsers != null)
                roomUsers.Add(StorageContext.Users.Where(u => u.userID == userID).Single());
        }
    }
}
