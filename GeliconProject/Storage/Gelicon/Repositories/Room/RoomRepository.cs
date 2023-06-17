using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions.Repositories.Room;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.Storage.Gelicon.Repositories.Room
{
    public class RoomRepository : IRoomRepository
    {
        private IStorageContext StorageContext { get; set; }

        public void SetStorageContext(IStorageContext storageContext)
        {
            StorageContext = storageContext;
        }

        public Models.Room GetRoomByID(int id)
        {
            return StorageContext.Rooms.Where(r => r.roomID == id)
                .Include(r => r.roomUsersColors!)
                .ThenInclude(r => r.color)
                .Include(r => r.users)
                .Single();
        }
    }
}
