using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GeliconProject.Models.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.ToTable("RefreshToken").HasKey(r => r.refreshTokenID);
            builder.ToTable("RefreshToken").Property("userID").IsRequired();
            builder.ToTable("RefreshToken").Property("token").IsRequired();
            builder.ToTable("RefreshToken").HasOne(r => r.user);
        }
    }
}
