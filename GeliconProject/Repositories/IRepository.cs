using GeliconProject.ApplicationContexts;
using GeliconProject.Models;

namespace GeliconProject.Repositories
{
    public interface IRepository
    {
        public int? SaveChanges();

        public User? GetUserByEmail(string email);

        public Task AddUser(User user);

        public List<Color> GetColors();

        public Room? GetRoomByID(int id);

        public User? GetUserByID(int id);

        public User? GetUserByIDWithoutJoins(int id);

        public Task AddMusicToRoom(string roomID, string musicID);
    }
}
