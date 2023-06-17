using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads
{
    public class RoomObserverThread : IRoomObserverThread
    {
        private event OnEmptyRoom? emptyRoomEvent;
        private Thread thread;
        private bool isInterrupted;
        private string? roomID;
        private List<IClientObserverThread> clientObservers;
        private IRoomMusicPlayer? roomMusicPlayer;
        
        public string? RoomID { get => roomID; set => roomID = value; }

        public RoomObserverThread()
        {
            clientObservers = new List<IClientObserverThread>();
            roomMusicPlayer = new RoomMusicPlayer.RoomMusicPlayer(clientObservers, new RoomMusicPlayer.Model.RoomMusicPlayerModel());
            isInterrupted = true;
            thread = new Thread(ThreadDelegate);
        }

        private async void ThreadDelegate()
        {
            while (!isInterrupted)
            {
                Thread.Sleep(5000);
            }
        }

        public void AddNewClient(string connectionID, IClientProxy client)
        {
            ClientObserverThread clientObserverThread = new ClientObserverThread(connectionID, client);
            clientObserverThread.AddConnectionLostHandler(ClientDisconnectHandler);
            clientObservers.Add(clientObserverThread);
            clientObserverThread.Start();
        }

        public void Start()
        {
            isInterrupted = false;
            thread.Start();
        }

        public async Task HandlePingResponse(string connectionID, DateTime responseReceived)
        {
            IClientObserverThread? clientObserverThread = clientObservers.Find(c => c.ConnectionID == connectionID);
            
            if (clientObserverThread != null)
                clientObserverThread.SetPingResult(responseReceived);
        }

        private void ClientDisconnectHandler(IClientObserverThread thread)
        {
            clientObservers.Remove(thread);
            if (clientObservers.Count == 0)
                emptyRoomEvent?.Invoke(this);
        }

        public void AddEmptyRoomHandler(OnEmptyRoom handler)
        {
            emptyRoomEvent += handler;
        }

        public void SetClientMusic(string connectionID)
        {
            IClientObserverThread? client = clientObservers.Find(c => c.ConnectionID == connectionID);
            if (client != null)
                roomMusicPlayer?.SetClientMusic(client);
        }
    
    }
}
