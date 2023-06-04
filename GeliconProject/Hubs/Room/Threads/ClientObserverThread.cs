using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Threads
{
    public class ClientObserverThread
    {
        private Thread thread;
        private bool isInterrupted;
        public string connectionID;
        private IClientProxy client;
        private DateTime lastPingSended;
        public int ping;

        public ClientObserverThread(string connectionID, IClientProxy client)
        {
            isInterrupted = true;
            thread = new Thread(ThreadDelegate);
            this.connectionID = connectionID;
            this.client = client;
            ping = -1;
        }

        private async void ThreadDelegate()
        {
            while (!isInterrupted)
            {
                await PingSend();
                Thread.Sleep(5000);
            }
        }

        private async Task PingSend()
        {
            lastPingSended = DateTime.Now;
            await client.SendAsync("PingReceive");
        }

        public void Start()
        {
            isInterrupted = false;
            thread.Start();
        }

        public void SetPingResult(DateTime responseReceived)
        {
            ping = (responseReceived - lastPingSended).Milliseconds;
        }
    }
}
