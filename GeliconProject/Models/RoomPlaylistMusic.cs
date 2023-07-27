using GeliconProject.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliconProject.Models
{
    [Table("RoomPlaylistMusic")]
    [EntityTypeConfiguration(typeof(RoomPlaylistMusicConfiguration))]
    public class RoomPlaylistMusic
    {
        public int roomPlaylistMusicID { get; set; }
        public int roomMusicID { get; set; }
        public int roomPlaylistID { get; set; }
        public RoomMusic? roomMusic { get; set; }
        public RoomPlaylist? roomPlaylist { get; set; }
    }
}
