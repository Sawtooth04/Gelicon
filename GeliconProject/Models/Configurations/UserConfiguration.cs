using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GeliconProject.Models;

namespace GeliconProject.Models.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User").Property("name").IsRequired();
            builder.ToTable("User").Property("email").IsRequired();
            builder.ToTable("User").Property("passwordHash").IsRequired();
            builder.ToTable("User").HasMany(u => u.rooms).WithMany(r => r.users);
            builder.ToTable("User").HasMany(u => u.ownRooms).WithOne(r => r.owner).HasForeignKey(r => r.ownerID);
        }
    }
}
