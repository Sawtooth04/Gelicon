using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeliconProject.Models.Configurations
{
    public class RoomPlaylistConfiguration : IEntityTypeConfiguration<RoomPlaylist>
    {
        public void Configure(EntityTypeBuilder<RoomPlaylist> builder)
        {
            builder.ToTable("RoomPlaylist").Property("roomID").IsRequired();
            builder.ToTable("RoomPlaylist").Property("name").IsRequired();
            builder.ToTable("RoomPlaylist").Property("addedAt").IsRequired();
            builder.ToTable("RoomPlaylist").HasOne(r => r.room).WithMany(r => r.roomPlaylists).HasForeignKey(r => r.roomID);
        }
    }
}
