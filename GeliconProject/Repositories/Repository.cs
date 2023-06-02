using GeliconProject.ApplicationContexts;
using GeliconProject.Models;
using GeliconProject.Utils.ApplicationContexts;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.Repositories
{
    public class Repository : IRepository
    {
        private IAppContext? context;

        public Repository(IAppContext appContext)
        {
            context = appContext;
        }

        public int? SaveChanges()
        {
            return context?.SaveChanges();
        }

        public User? GetUserByEmail(string email)
        {
            User? user = context?.Users.Where(user => user.email == email).First();
            context?.DetachAll();
            return user;
        }

        public async Task AddUser(User user)
        {
            if (context != null)
                await Task.Run(() => context.Users.AddAsync(user));
        }

        public List<Color> GetColors()
        {
            List<Color> colors;
            if (context != null)
            {
                colors = context.Colors.ToList();
                context.DetachAll();
                return colors;
            }
            return new List<Color>();
        }

        public Room? GetRoomByID(int id)
        {
            Room? room;

            if (context != null)
            {
                room = context.Rooms.Where(r => r.roomID == id)
                    .Include(r => r.roomUsersColors!)
                    .ThenInclude(r => r.color)
                    .Include(r => r.users).FirstOrDefault();
                context.DetachAll();
                return room;
            }
            return null;
        }

        public User? GetUserByID(int id)
        {
            User? user;

            if (context != null)
            {
                user = context.Users.Where(u => u.userID == id).Include(u => u.rooms)!.ThenInclude(r => r.owner).FirstOrDefault();
                context?.DetachAll();
                return user;
            }
            return null;
        }

        public User? GetUserByIDWithoutJoins(int id)
        {
            User? user;

            if (context != null)
            {
                user = context.Users.Where(u => u.userID == id).FirstOrDefault();
                context?.DetachAll();
                return user;
            }
            return null;
        }
    }
}
