using GeliconProject.Models;

namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models
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
        public bool PlayNext { get; set; }
        public RoomPlaylist? currentPlaylist { get; set; }

        public void AddClientModel(string connectionID);

        public void RemoveClientModel(string connectionID);

        public IClientRoomMusicPlayerModel? GetClientModel(string connectionID);

        public void SetClientCurrentTime(string connectionID, double value);

        public List<KeyValuePair<string, IClientRoomMusicPlayerModel>> GetClientRoomMusicPlayerModels();
    }
}
