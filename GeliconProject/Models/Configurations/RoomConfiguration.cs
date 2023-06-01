using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeliconProject.Models.Configurations
{
    public class RoomConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.ToTable("Room").HasKey(r => r.roomID);
            builder.ToTable("Room").Property("name").IsRequired();
            builder.ToTable("Room").Property("ownerID").IsRequired();
            builder.ToTable("Room").Property("password").IsRequired();
            builder.ToTable("Room").HasMany(r => r.users).WithMany(u => u.rooms);
            builder.ToTable("Room").HasOne(r => r.owner).WithMany(u => u.ownRooms).HasForeignKey(r => r.ownerID);
            builder.ToTable("Room").HasMany(r => r.roomUsersColors).WithOne(r => r.room);
        }
    }
}
