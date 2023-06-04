using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Threads
{
    public class RoomObserverThread
    {
        private Thread thread;
        private bool isInterrupted;
        private string roomID;
        public IHubCallerClients? clients;
        private List<ClientObserverThread> clientObservers;
        public string RoomID { get => roomID; }

        public RoomObserverThread(string roomID)
        {
            clientObservers = new List<ClientObserverThread>();
            this.roomID = roomID;
            isInterrupted = true;
            thread = new Thread(ThreadDelegate);
        }

        private async void ThreadDelegate(object? args)
        {
            IHubCallerClients? clients = args as IHubCallerClients;

            while (!isInterrupted)
            {
                Thread.Sleep(5000);
            }
        }

        public void AddNewClient(string connectionID)
        {
            if (clients != null)
            {
                ClientObserverThread clientObserverThread = new ClientObserverThread(connectionID, clients.Client(connectionID));
                clientObservers.Add(clientObserverThread);
                clientObserverThread.Start();
            }
        }

        public void Start()
        {
            isInterrupted = false;
            thread.Start(clients);
        }

        public async Task HandlePingResponse(string connectionID, DateTime responseReceived)
        {
            ClientObserverThread? clientObserverThread = clientObservers.Find(c => c.connectionID == connectionID);
            
            if (clientObserverThread != null)
            {
                clientObserverThread.SetPingResult(responseReceived);
                await clients.Client(connectionID).SendAsync("LogPing", clientObserverThread.ping);
            }
        }
    }
}
