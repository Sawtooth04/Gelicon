using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Controllers;
using GeliconProject.Hubs.Room.Abstractions.Threads.ClientThread;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads.ClientThread
{
    public class ClientThread : IClientThread
    {
        private event OnConnectionLost? connectionLostEvent;
        private IRoomMusicPlayerSynchronizationMediator synchronizationMediator;
        private Thread thread;
        private bool isInterrupted;
        private IClientProxy client;
        private DateTime pingSended;
        private sbyte pingAttempts;
        private int userID;

        public int Ping { get; set; }
        public string ConnectionID { get; }
        public IClientProxy Client { get => client; }
        public int UserID { get => userID; }

        public ClientThread(string connectionID, int userID, IClientProxy client, IRoomMusicPlayerSynchronizationMediator synchronizationMediator)
        {
            isInterrupted = true;
            thread = new Thread(ThreadDelegate);
            ConnectionID = connectionID;
            this.client = client;
            this.synchronizationMediator = synchronizationMediator;
            Ping = 0;
            this.userID = userID;
            pingAttempts = -1;
        }

        private void ThreadDelegate()
        {
            try
            {
                while (!isInterrupted)
                {
                    SendPing();
                    Thread.Sleep(2500);
                }
            }
            catch (Exception)
            {

            }
        }

        private void SendPing()
        {
            if (pingAttempts == 0)
            {
                pingSended = DateTime.Now;
                synchronizationMediator.InvokeSendPing(Client);
            }
            else if (pingAttempts >= 5)
            {
                Interrupt();
                connectionLostEvent?.Invoke(this);
            }
            pingAttempts++;
        }

        public void Start()
        {
            isInterrupted = false;
            thread.Start();
        }

        public void Interrupt()
        {
            thread.Interrupt();
        }

        public void SetPingResult(DateTime responseReceived)
        {
            Ping = (responseReceived - pingSended).Milliseconds;
            pingAttempts = 0;
        }

        public void AddConnectionLostHandler(OnConnectionLost handler)
        {
            connectionLostEvent += handler;
        }
    }
}
