namespace GeliconProject.Hubs.Room.RoomMusicPlayer
{
    public class RoomMusicPlayer : IRoomMusicPlayer
    {
        public string CurrentMusicID { get; set; }

        public RoomMusicPlayer()
        {
            CurrentMusicID = "";
        }
    }
}
