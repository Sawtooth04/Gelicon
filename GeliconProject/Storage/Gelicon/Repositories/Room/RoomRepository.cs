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
                .Include(r => r.users)
                .Single();
        }

        public void AddRoom(Models.Room room)
        {
            StorageContext.Rooms.Add(room);
        }

        public bool IsUnique(string name)
        {
            return StorageContext.Rooms.Where(r => r.name == name).Count() == 0;
        }
    }
}
