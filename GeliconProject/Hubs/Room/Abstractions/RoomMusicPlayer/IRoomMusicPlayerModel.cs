using GeliconProject.Models;

namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer
{
    public enum RoomMusicPlayerSource
    {
        Music,
        Playlist
    }

    public interface IRoomMusicPlayerModel
    {
        public RoomMusic CurrentMusic { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsDescending { get; set; }
        public RoomMusicPlayerSource Source { get; set; }
    }
}
