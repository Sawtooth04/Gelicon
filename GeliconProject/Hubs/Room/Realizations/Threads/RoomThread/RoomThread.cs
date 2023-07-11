using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Hubs.Room.Abstractions.Threads.ClientThread;
using GeliconProject.Hubs.Room.Abstractions.Threads.RoomThread;
using GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Models;
using GeliconProject.Storage.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads.RoomThread
{
    public class RoomThread : IRoomThread
    {
        private event OnEmptyRoom? emptyRoomEvent;
        private Thread thread;
        private bool isInterrupted;
        private string? roomID;
        private List<IClientThread> clientThreads;
        private IRoomMusicPlayerModel? roomMusicPlayerModel;
        private IRoomMusicPlayerSynchronizationMediator synchronizationMediator;

        public string? RoomID { get => roomID; set => roomID = value; }

        public IRoomMusicPlayerModel? RoomMusicPlayerModel { get => roomMusicPlayerModel; set => roomMusicPlayerModel = value; }

        public RoomThread(IRoomMusicPlayerSynchronizationMediator synchronizationMediator)
        {
            this.synchronizationMediator = synchronizationMediator;
            clientThreads = new List<IClientThread>();
            roomMusicPlayerModel = new RoomMusicPlayerModel();
            isInterrupted = true;
            thread = new Thread(ThreadDelegate);
        }

        private void ThreadDelegate()
        {
            while (!isInterrupted)
            {
                if (roomMusicPlayerModel != null && roomMusicPlayerModel!.IsPlaying)
                {
                    SendCurrentTimePing();
                    Synchronize();
                }
                Thread.Sleep(2500);
            }
        }

        private void Synchronize()
        {
            if (clientThreads.Count > 1)
            {
                synchronizationMediator.InvokeSynchronizationModel(roomMusicPlayerModel!);
                foreach (IClientThread clientThread in clientThreads)
                    synchronizationMediator.InvokeSynchronizationClient(
                        clientThread.ConnectionID,
                        clientThread.Client,
                        roomMusicPlayerModel!,
                        clientThread.Ping
                    );
            }
        }

        public void AddNewClient(string connectionID, IClientProxy client)
        {
            IClientThread clientThread = new ClientThread.ClientThread(connectionID, client, synchronizationMediator);
            clientThread.AddConnectionLostHandler(ClientDisconnectHandler);
            clientThreads.Add(clientThread);
            clientThread.Start();
            roomMusicPlayerModel?.AddClientModel(connectionID);
        }

        public void Start()
        {
            isInterrupted = false;
            thread.Start();
        }

        public void HandlePingResponse(string connectionID, DateTime responseReceived)
        {
            IClientThread? clientThread = clientThreads.Find(c => c.ConnectionID == connectionID);

            if (clientThread != null)
                clientThread.SetPingResult(responseReceived);
        }

        private void ClientDisconnectHandler(IClientThread thread)
        {
            clientThreads.Remove(thread);
            roomMusicPlayerModel?.RemoveClientModel(thread.ConnectionID);
            if (clientThreads.Count == 0)
                emptyRoomEvent?.Invoke(this);
        }

        public void AddEmptyRoomHandler(OnEmptyRoom handler)
        {
            emptyRoomEvent += handler;
        }

        private void SendCurrentTimePing()
        {
            foreach (IClientThread client in clientThreads)
                synchronizationMediator.InvokeSendCurrentTimePing(client.Client);
        }
    }
}
