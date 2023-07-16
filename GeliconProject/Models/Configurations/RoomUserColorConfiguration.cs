using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeliconProject.Models.Configurations
{
    public class RoomUserColorConfiguration : IEntityTypeConfiguration<RoomUserColor>
    {
        public void Configure(EntityTypeBuilder<RoomUserColor> builder)
        {
            builder.ToTable("RoomUserColor").Property("userID").IsRequired();
            builder.ToTable("RoomUserColor").Property("color").HasMaxLength(7).IsRequired();
            builder.ToTable("RoomUserColor").Property("roomID").IsRequired();
            builder.ToTable("RoomUserColor").HasOne(uc => uc.user).WithMany(u => u.userColors);
            builder.ToTable("RoomUserColor").HasOne(uc => uc.room).WithMany(r => r.roomUsersColors);
        }
    }
}
