namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models
{
    public interface IRoomMusicPlayerSynchronizationModel
    {
        public double GetCurrentTime(IRoomMusicPlayerModel roomMusicPlayerModel);

        public void SynchronizeModels(IRoomMusicPlayerModel roomMusicPlayerModel);
    }
}
