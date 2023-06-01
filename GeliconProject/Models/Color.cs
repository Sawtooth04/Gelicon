using GeliconProject.Models.Configurations;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeliconProject.Models
{
    [Table("Color")]
    [EntityTypeConfiguration(typeof(ColorConfiguration))]
    public class Color
    {
        public int colorID { get; set; }
        public string? name { get; set; }
        public List<RoomUserColor>? userColors { get; set; }
    }
}
