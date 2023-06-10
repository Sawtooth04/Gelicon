using Microsoft.EntityFrameworkCore;
using GeliconProject.Models;
using GeliconProject.ApplicationContexts;

namespace GeliconProject.Utils.ApplicationContexts
{
    public class ApplicationContext : DbContext, IAppContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Color> Colors => Set<Color>();
        public DbSet<RoomMusic> RoomMusics => Set<RoomMusic>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public void DetachAll()
        {
            ChangeTracker.Clear();
        }
    }
}
