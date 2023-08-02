using GeliconProject.Models;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.Storage.Abstractions.Context
{
    public interface IStorageContext
    {
        public DbSet<User> Users { get; }
        public DbSet<Room> Rooms { get; }
        public DbSet<RoomMusic> RoomMusics { get; }
        public DbSet<RefreshToken> RefreshTokens { get; }
        public DbSet<RoomPlaylist> RoomPlaylists { get; }
        public DbSet<RoomPlaylistMusic> RoomPlaylistsMusic { get; }

        public int SaveChanges();

        public void DetachAll();
    }
}
