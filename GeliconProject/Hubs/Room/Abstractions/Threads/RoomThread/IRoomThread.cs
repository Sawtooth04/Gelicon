using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.RoomThread
{
    public delegate void OnEmptyRoom(IRoomThread thread);

    public interface IRoomThread
    {
        public string? RoomID { get; set; }

        public IRoomMusicPlayerModel? RoomMusicPlayerModel { get; set; }

        public void AddNewClient(string connectionID, int userID, IClientProxy client);

        public void Start();

        public void HandlePingResponse(string connectionID, DateTime responseReceived);

        public void AddEmptyRoomHandler(OnEmptyRoom handler);

        public List<int> GetOnlineUsersID();
    }
}
