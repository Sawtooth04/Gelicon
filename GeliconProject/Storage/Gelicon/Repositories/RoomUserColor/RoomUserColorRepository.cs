using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions.Repositories.RoomUserColor;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.Storage.Gelicon.Repositories.RoomUserColor
{
    public class RoomUserColorRepository : IRoomUserColorRepository
    {
        private IStorageContext storageContext;

        public void SetStorageContext(IStorageContext storageContext)
        {
            this.storageContext = storageContext;
        }

        public void ChangeRoomUserColor(int roomID, int userID, string color)
        {
            Models.RoomUserColor? roomUserColor = storageContext.Rooms.Where(r => r.roomID == roomID).Include(r => r.roomUsersColors).Single()
                .roomUsersColors!.Where(r => r.userID == userID).FirstOrDefault();

            if (roomUserColor != null)
                roomUserColor.color = color;
        }

        public async Task<List<Models.RoomUserColor>> GetRoomUsersColor(int roomID)
        {
            return (await storageContext.Rooms.Where(r => r.roomID == roomID).Include(r => r.roomUsersColors).SingleAsync()).roomUsersColors!;
        }

        public void AddDefaultRoomUserColor(Models.Room room, Models.User user)
        {
            room.roomUsersColors!.Add(new Models.RoomUserColor()
            {
                room = room,
                user = user,
                color = room.defaultColor
            });
        }
    }
}
