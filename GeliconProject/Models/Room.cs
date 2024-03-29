﻿using GeliconProject.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliconProject.Models
{
    [Table("Room")]
    [EntityTypeConfiguration(typeof(RoomConfiguration))]
    public class Room
    {
        public int roomID { get; set; }
        public string? name { get; set; }
        public int ownerID { get; set; }
        public string? defaultColor { get; set; }
        public User? owner { get; set; }
        public List<User>? users { get; set; }
        public List<RoomUserColor>? roomUsersColors { get; set; }
        public List<RoomMusic>? roomMusics { get; set; }
        public List<RoomPlaylist>? roomPlaylists { get; set; }
    }
}
