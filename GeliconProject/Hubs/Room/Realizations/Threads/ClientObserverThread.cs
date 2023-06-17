using GeliconProject.Hubs.Room.Abstractions.Threads;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads
{
    public class ClientObserverThread : IClientObserverThread
    {
        private event OnConnectionLost? connectionLostEvent;
        private Thread thread;
        private bool isInterrupted;
        private IClientProxy client;
        private DateTime pingSended;
        private sbyte pingAttempts;

        public int Ping { get; set; }
        public string ConnectionID { get; }

        public ClientObserverThread(string connectionID, IClientProxy client)
        {
            isInterrupted = true;
            thread = new Thread(ThreadDelegate);
            this.ConnectionID = connectionID;
            this.client = client;
            Ping = 0;
            pingAttempts = -1;
        }

        private async void ThreadDelegate()
        {
            try
            {
                while (!isInterrupted)
                {
                    await PingSend();
                    Thread.Sleep(3000);
                }
            }
            catch (Exception)
            {

            }
        }

        private async Task PingSend()
        {
            if (pingAttempts == 0)
            {
                pingSended = DateTime.Now;
                await client.SendAsync("PingReceive");
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

        public async Task SendAsync(string method, object? arg)
        {
            await client.SendAsync(method, arg);
        }
    }
}
