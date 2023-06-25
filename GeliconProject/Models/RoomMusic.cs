using GeliconProject.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliconProject.Models
{
    [Table("RoomMusic")]
    [EntityTypeConfiguration(typeof(RoomMusicConfiguration))]
    public class RoomMusic
    {
        public int roomMusicID { get; set; }
        public int roomID { get; set; }
        public string? musicID { get; set; }
        public DateTime addedAt { get; set; }
        public Room? room { get; set; }
    }
}
