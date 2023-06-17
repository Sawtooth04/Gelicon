﻿using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Abstractions.Threads
{
    public delegate void OnEmptyRoom(IRoomObserverThread thread);

    public interface IRoomObserverThread
    {
        public string? RoomID { get; set; }

        public void AddNewClient(string connectionID, IClientProxy client);

        public void Start();

        public Task HandlePingResponse(string connectionID, DateTime responseReceived);

        public void AddEmptyRoomHandler(OnEmptyRoom handler);

        public void SetClientMusic(string connectionID);
    }
}