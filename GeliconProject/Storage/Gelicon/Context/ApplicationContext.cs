using GeliconProject.Models;
using GeliconProject.Storage.Abstractions.Context;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.Storage.Gelicon.Context
{
    public class ApplicationContext : DbContext, IStorageContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Room> Rooms => Set<Room>();
        public DbSet<RoomMusic> RoomMusics => Set<RoomMusic>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

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
