using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeliconProject.Models.Configurations
{
    public class ColorConfiguration : IEntityTypeConfiguration<Color>
    {
        public void Configure(EntityTypeBuilder<Color> builder)
        {
            builder.ToTable("Color").Property("name").IsRequired();
            builder.ToTable("Color").HasMany(c => c.userColors).WithOne(uc => uc.color);
        }
    }
}
