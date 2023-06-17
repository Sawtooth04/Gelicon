namespace GeliconProject.Hubs.Room.Abstractions.Threads
{
    public delegate void OnConnectionLost(IClientObserverThread thread);

    public interface IClientObserverThread
    {
        public string ConnectionID { get; }
        public int Ping { get; set; }

        public void Start();

        public void Interrupt();

        public void SetPingResult(DateTime responseReceived);

        public void AddConnectionLostHandler(OnConnectionLost handler);

        public Task SendAsync(string method, object? arg);
    }
}
