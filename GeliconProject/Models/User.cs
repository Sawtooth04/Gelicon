using GeliconProject.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliconProject.Models
{
    [Table("User")]
    [EntityTypeConfiguration(typeof(UserConfiguration))]
    public class User
    {
        public int userID { get; set; }
        public string? name { get; set; }
        public string? passwordHash { get; set; }
        public string? email { get; set; }
        public List<Room>? rooms { get; set; }
        public List<Room>? ownRooms { get; set; }
        public List<RoomUserColor>? userColors { get; set; }
    }
}
