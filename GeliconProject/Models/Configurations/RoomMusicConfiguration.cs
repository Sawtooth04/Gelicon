using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeliconProject.Models.Configurations
{
    public class RoomMusicConfiguration : IEntityTypeConfiguration<RoomMusic>
    {
        public void Configure(EntityTypeBuilder<RoomMusic> builder)
        {
            builder.ToTable("RoomMusic").Property("roomID").IsRequired();
            builder.ToTable("RoomMusic").Property("musicID").IsRequired();
            builder.ToTable("RoomMusic").HasOne(r => r.room).WithMany(r => r.roomMusics).HasForeignKey(r => r.roomID);
        }
    }
}
