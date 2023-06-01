using Microsoft.EntityFrameworkCore;
using GeliconProject.Models;

namespace GeliconProject.Utils.ApplicationContexts
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<Color> Colors => Set<Color>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
