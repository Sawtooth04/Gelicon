using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Model;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Model
{
    public class RoomMusicPlayerModel : IRoomMusicPlayerModel
    {
        private string currentMusic;
        public string CurrentMusic { get => currentMusic; }

        public RoomMusicPlayerModel()
        {
            currentMusic = "wGxZ2";
        }
    }
}
