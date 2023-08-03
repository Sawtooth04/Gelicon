using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Models;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Models
{
    public class RoomMusicPlayerModel : IRoomMusicPlayerModel
    {
        private Dictionary<string, IClientRoomMusicPlayerModel> clientsModels { get; set; }
        public RoomMusic CurrentMusic { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsDescending { get; set; }
        public RoomMusicPlayerSource Source { get; set; }
        public bool PlayNext { get; set; }
        public RoomPlaylist? currentPlaylist { get; set; }

        public RoomMusicPlayerModel()
        {
            CurrentMusic = new RoomMusic();
            IsPlaying = false;
            IsDescending = true;
            Source = RoomMusicPlayerSource.Music;
            clientsModels = new Dictionary<string, IClientRoomMusicPlayerModel>();
            PlayNext = true;
        }

        public void AddClientModel(string connectionID)
        {
            clientsModels.Add(connectionID, new ClientRoomMusicPlayerModel());
        }

        public void RemoveClientModel(string connectionID)
        {
            clientsModels.Remove(connectionID);
        }

        public IClientRoomMusicPlayerModel? GetClientModel(string connectionID)
        {
            IClientRoomMusicPlayerModel? result;
            if (clientsModels.TryGetValue(connectionID, out result))
                return result;
            return null;
        }

        public void SetClientCurrentTime(string connectionID, double value)
        {
            IClientRoomMusicPlayerModel? clientModel = GetClientModel(connectionID);
            if (clientModel != null)
                clientModel.CurrentTime = value;
        }

        public List<KeyValuePair<string, IClientRoomMusicPlayerModel>> GetClientRoomMusicPlayerModels()
        {
            return clientsModels.ToList();
        }
    }
}
