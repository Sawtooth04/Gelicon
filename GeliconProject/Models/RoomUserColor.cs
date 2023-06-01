using GeliconProject.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliconProject.Models
{
    [Table("RoomUserColor")]
    [EntityTypeConfiguration(typeof(RoomUserColorConfiguration))]
    public class RoomUserColor
    {
        public int roomUserColorID { get; set; }
        public int userID { get; set; }
        public int colorID { get; set; }
        public int roomID { get; set; }
        public User? user { get; set; }
        public Room? room { get; set; }
        public Color? color { get; set; }
    }
}
