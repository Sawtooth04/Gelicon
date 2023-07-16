using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.Room
{
    public interface IRoomRepository : IRepository
    {
        public Models.Room GetRoomByID(int id);
        public void AddRoom(Models.Room room);
        public bool IsUnique(string name);
    }
}
