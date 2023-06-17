using GeliconProject.Hubs.Room.Abstractions.Threads;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer
{
    public delegate void OnUserJoin(IClientObserverThread clientObserver);

    public interface IRoomMusicPlayer
    {
        public void SetClientMusic(IClientObserverThread clientObserver);
    }
}
