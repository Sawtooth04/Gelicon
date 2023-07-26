using GeliconProject.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliconProject.Models
{
    [Table("RefreshToken")]
    [EntityTypeConfiguration(typeof(RefreshTokenConfiguration))]
    public class RefreshToken
    {
        public int refreshTokenID { get; set; }
        public int userID { get; set; }
        public string? token { get; set; }
        public User? user { get; set; }
    }
}
