using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Models;
using GeliconProject.Storage.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads
{
    public class RoomThread : IRoomThread
    {
        private event OnEmptyRoom? emptyRoomEvent;
        private Thread thread;
        private bool isInterrupted;
        private string? roomID;
        private List<IClientThread> clientObservers;
        private IRoomMusicPlayerModel? roomMusicPlayerModel;
        private IRoomMusicPlayerSynchronizationMediator synchronizationMediator;

        public string? RoomID { get => roomID; set => roomID = value; }

        public IRoomMusicPlayerModel? RoomMusicPlayerModel { get => roomMusicPlayerModel; set => roomMusicPlayerModel = value; }

        public RoomThread(IRoomMusicPlayerSynchronizationMediator synchronizationMediator) 
        {
            this.synchronizationMediator = synchronizationMediator;
            clientObservers = new List<IClientThread>();
            roomMusicPlayerModel = new RoomMusicPlayerModel();
            isInterrupted = true;
            thread = new Thread(ThreadDelegate);
        }

        private async void ThreadDelegate()
        {
            while (!isInterrupted)
            {
                if (roomMusicPlayerModel!.IsPlaying)
                {
                    SendCurrentTimePing();
                    if (roomMusicPlayerModel != null)
                    {
                        synchronizationMediator.InvokeSynchronizationModel(roomMusicPlayerModel);
                        foreach (IClientThread clientThread in clientObservers)
                            synchronizationMediator.InvokeSynchronizationClient(
                                clientThread.ConnectionID,
                                clientThread.Client,
                                roomMusicPlayerModel,
                                clientThread.Ping
                            );
                    }
                }
                Thread.Sleep(2500);
            }
        }

        public void AddNewClient(string connectionID, IClientProxy client)
        {
            ClientThread clientObserverThread = new ClientThread(connectionID, client);
            clientObserverThread.AddConnectionLostHandler(ClientDisconnectHandler);
            clientObservers.Add(clientObserverThread);
            clientObserverThread.Start();
            roomMusicPlayerModel?.AddClientModel(connectionID);
        }

        public void Start()
        {
            isInterrupted = false;
            thread.Start();
        }

        public void HandlePingResponse(string connectionID, DateTime responseReceived)
        {
            IClientThread? clientObserverThread = clientObservers.Find(c => c.ConnectionID == connectionID);
            
            if (clientObserverThread != null)
                clientObserverThread.SetPingResult(responseReceived);
        }

        private void ClientDisconnectHandler(IClientThread thread)
        {
            clientObservers.Remove(thread);
            roomMusicPlayerModel?.RemoveClientModel(thread.ConnectionID);
            if (clientObservers.Count == 0)
                emptyRoomEvent?.Invoke(this);
            roomMusicPlayerModel?.RemoveClientModel(thread.ConnectionID);
        }

        public void AddEmptyRoomHandler(OnEmptyRoom handler)
        {
            emptyRoomEvent += handler;
        }

        private void SendCurrentTimePing()
        {
            foreach (IClientThread client in clientObservers)
                client.SendCurrentTimePing();
        }
    }
}
