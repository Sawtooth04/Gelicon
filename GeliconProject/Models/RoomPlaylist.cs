using GeliconProject.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliconProject.Models
{
    [Table("RoomPlaylist")]
    [EntityTypeConfiguration(typeof(RoomPlaylistConfiguration))]
    public class RoomPlaylist
    {
        public int roomPlaylistID { get; set; }
        public int roomID { get; set; }
        public string? name { get; set; }
        public DateTime addedAt { get; set; }
        public Room? room { get; set; }
        public List<RoomPlaylistMusic>? roomPlaylistMusics { get; set; }
    }
}
