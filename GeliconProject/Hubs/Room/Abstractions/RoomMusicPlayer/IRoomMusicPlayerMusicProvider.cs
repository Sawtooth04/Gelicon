﻿using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;

namespace GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer
{
    public interface IRoomMusicPlayerMusicProvider
    {
        public Func<IStorage, int, RoomMusic, bool, RoomMusic> GetNextMusicAction(RoomMusicPlayerSource source);
        public Func<IStorage, int, RoomMusic, bool, RoomMusic> GetPreviousMusicAction(RoomMusicPlayerSource source);
    }
}
