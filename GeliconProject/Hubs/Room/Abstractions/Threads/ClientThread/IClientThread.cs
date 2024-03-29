﻿using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Threads.ClientThread
{
    public delegate void OnConnectionLost(IClientThread thread);

    public interface IClientThread
    {
        public int Ping { get; set; }
        public string ConnectionID { get; }
        public IClientProxy Client { get; }
        public int UserID { get; }

        public void Start();

        public void Interrupt();

        public void SetPingResult(DateTime responseReceived);

        public void AddConnectionLostHandler(OnConnectionLost handler);
    }
}
