using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Threads
{
    public delegate void OnConnectionLost(ClientObserverThread thread);

    public class ClientObserverThread
    {
        private Thread thread;
        private bool isInterrupted;
        public string connectionID;
        private IClientProxy client;
        private DateTime pingSended;
        private sbyte pingAttempts;
        public int ping;
        private event OnConnectionLost? connectionLostEvent; 

        public ClientObserverThread(string connectionID, IClientProxy client)
        {
            isInterrupted = true;
            thread = new Thread(ThreadDelegate);
            this.connectionID = connectionID;
            this.client = client;
            ping = -1;
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
            ping = (responseReceived - pingSended).Milliseconds;
            pingAttempts = 0;
        }

        public void addConnectionLostHandler(OnConnectionLost handler)
        {
            connectionLostEvent += handler;
        }
    }
}
