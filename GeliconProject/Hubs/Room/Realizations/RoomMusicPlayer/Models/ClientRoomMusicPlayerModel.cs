using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Models
{
    public class ClientRoomMusicPlayerModel : IClientRoomMusicPlayerModel
    {
        public double CurrentTime { get; set; }
        public bool NeedToRewind { get; set; }
        public double SpeedRatio { get; set; }

        public ClientRoomMusicPlayerModel()
        {
            CurrentTime = 0;
            NeedToRewind = false;
            SpeedRatio = 1;
        }
    }
}
