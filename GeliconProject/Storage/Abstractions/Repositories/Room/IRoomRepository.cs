using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.Room
{
    public interface IRoomRepository : IRepository
    {
        public Models.Room GetRoomByID(int id);
    }
}
