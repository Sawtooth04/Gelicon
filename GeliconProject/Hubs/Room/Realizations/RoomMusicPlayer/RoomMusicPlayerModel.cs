using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Models;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer
{
    public class RoomMusicPlayerModel : IRoomMusicPlayerModel
    {
        public RoomMusic CurrentMusic { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsDescending { get; set; }
        public RoomMusicPlayerSource Source { get; set; }

        public RoomMusicPlayerModel()
        {
            CurrentMusic = new RoomMusic();
            IsPlaying = false;
            IsDescending = true;
            Source = RoomMusicPlayerSource.Music;
        }
    }
}
