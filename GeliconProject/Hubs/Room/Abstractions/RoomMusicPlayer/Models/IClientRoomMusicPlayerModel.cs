namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models
{
    public interface IClientRoomMusicPlayerModel
    {
        public double CurrentTime { get; set; }
        public bool NeedToRewind { get; set; }
        public double SpeedRatio { get; set; }
    }
}
