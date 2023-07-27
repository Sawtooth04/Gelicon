using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeliconProject.Models.Configurations
{
    public class RoomPlaylistMusicConfiguration : IEntityTypeConfiguration<RoomPlaylistMusic>
    {
        public void Configure(EntityTypeBuilder<RoomPlaylistMusic> builder)
        {
            builder.ToTable("RoomPlaylistMusic").Property("roomMusicID").IsRequired();
            builder.ToTable("RoomPlaylistMusic").Property("roomPlaylistID").IsRequired();
            builder.ToTable("RoomPlaylistMusic").HasOne(r => r.roomMusic).WithMany(r => r.roomPlaylistsMusic).HasForeignKey(r => r.roomMusicID);
            builder.ToTable("RoomPlaylistMusic").HasOne(r => r.roomPlaylist).WithMany(r => r.roomPlaylistMusics).HasForeignKey(r => r.roomPlaylistID);
        }
    }
}
