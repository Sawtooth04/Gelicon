﻿using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.RoomMusic
{
    public interface IRoomMusicRepository : IRepository
    {
        public Task Add(int roomID, string musicID);

        public Task<List<Models.RoomMusic>> GetRoomMusic(int roomID, int start, int count);

        public Models.RoomMusic? GetRoomMusic(int roomID, string musicID);

        public Models.RoomMusic GetLatestAddedMusic(int roomID, bool isDescending = true);

        public Models.RoomMusic GetNextMusic(int roomID, Models.RoomMusic currentMusic, bool isDescending = true);

        public Models.RoomMusic GetPreviousMusic(int roomID, Models.RoomMusic currentMusic, bool isDescending = true);

        public void DeleteRoomMusic(int roomID, string musicID);
    }
}
