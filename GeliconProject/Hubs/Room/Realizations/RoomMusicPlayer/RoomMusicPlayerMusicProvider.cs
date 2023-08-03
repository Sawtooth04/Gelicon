using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.RoomMusic;
using GeliconProject.Storage.Abstractions.Repositories.RoomPlaylistMusic;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer
{
    public class RoomMusicPlayerMusicProvider : IRoomMusicPlayerMusicProvider
    {
        private Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>> nextMusicActions;
        private Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>> previousMusicActions;
        private Dictionary<RoomMusicPlayerSource, Func<IStorage, int, string, RoomMusic?>> getMusicActions;
        private Dictionary<RoomMusicPlayerSource, Action<IStorage, int, string>> deleteMusicActions;
        private Dictionary<RoomMusicPlayerSource, Func<IStorage, int, string, Task>> addMusicActions;

        public RoomMusicPlayerMusicProvider()
        {
            nextMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>>();
            previousMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>>();
            getMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, string, RoomMusic?>>();
            deleteMusicActions = new Dictionary<RoomMusicPlayerSource, Action<IStorage, int, string>>();
            addMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, string, Task>>();
            ConfigureNextMusicActions();
            ConfigurePreviousMusicActions();
            ConfigureGetMusicActions();
            ConfigureDeleteMusicActions();
            ConfigureAddMusicActions();
        }

        public Func<IStorage, int, RoomMusic, bool, RoomMusic> GetNextMusicAction(RoomMusicPlayerSource source)
        {
            Func<IStorage, int, RoomMusic, bool, RoomMusic>? result;

            if (nextMusicActions.TryGetValue(source, out result))
                return result;
            throw new Exception("Cannot find action for this source.");
        }

        public Func<IStorage, int, RoomMusic, bool, RoomMusic> GetPreviousMusicAction(RoomMusicPlayerSource source)
        {
            Func<IStorage, int, RoomMusic, bool, RoomMusic>? result;

            if (previousMusicActions.TryGetValue(source, out result))
                return result;
            throw new Exception("Cannot find action for this source.");
        }

        public Func<IStorage, int, string, RoomMusic?> GetGetMusicAction(RoomMusicPlayerSource source)
        {
            Func<IStorage, int, string, RoomMusic?>? result;

            if (getMusicActions.TryGetValue(source, out result))
                return result;
            throw new Exception("Cannot find action for this source.");
        }

        public Action<IStorage, int, string> GetDeleteMusicAction(RoomMusicPlayerSource source)
        {
            Action<IStorage, int, string>? result;

            if (deleteMusicActions.TryGetValue(source, out result))
                return result;
            throw new Exception("Cannot find action for this source.");
        }

        public Func<IStorage, int, string, Task> GetAddMusicAction(RoomMusicPlayerSource source)
        {
            Func<IStorage, int, string, Task>? result;

            if (addMusicActions.TryGetValue(source, out result))
                return result;
            throw new Exception("Cannot find action for this source.");
        }

        private void ConfigureNextMusicActions()
        {
            nextMusicActions.Add(RoomMusicPlayerSource.Music, GetNextMusicInMusicList);
            nextMusicActions.Add(RoomMusicPlayerSource.Playlist, GetNextMusicInPlaylist);
        }

        private void ConfigurePreviousMusicActions()
        {
            previousMusicActions.Add(RoomMusicPlayerSource.Music, GetPreviousMusicInMusicList);
            previousMusicActions.Add(RoomMusicPlayerSource.Playlist, GetPreviousMusicInPlaylist);
        }

        private void ConfigureGetMusicActions()
        {
            getMusicActions.Add(RoomMusicPlayerSource.Music, GetRoomMusicInMusicList);
            getMusicActions.Add(RoomMusicPlayerSource.Playlist, GetRoomMusicInPlaylist);
        }

        private void ConfigureDeleteMusicActions()
        {
            deleteMusicActions.Add(RoomMusicPlayerSource.Music, DeleteRoomMusicInMusicList);
        }

        private void ConfigureAddMusicActions()
        {
            addMusicActions.Add(RoomMusicPlayerSource.Music, AddRoomMusicInMusicList);
        }

        private RoomMusic GetNextMusicInMusicList(IStorage storage, int roomID, RoomMusic currentMusic, bool isDescending = true)
        {
            return storage.GetRepository<IRoomMusicRepository>().GetNextMusic(roomID, currentMusic, isDescending);
        }

        private RoomMusic GetPreviousMusicInMusicList(IStorage storage, int roomID, RoomMusic currentMusic, bool isDescending = true)
        {
            return storage.GetRepository<IRoomMusicRepository>().GetPreviousMusic(roomID, currentMusic, isDescending);
        }

        private RoomMusic? GetRoomMusicInMusicList(IStorage storage, int roomID, string musicID)
        {
            return storage.GetRepository<IRoomMusicRepository>().GetRoomMusic(roomID, musicID);
        }

        private void DeleteRoomMusicInMusicList(IStorage storage, int roomID, string musicID)
        {
            storage.GetRepository<IRoomMusicRepository>().DeleteRoomMusic(roomID, musicID);
        }

        private async Task AddRoomMusicInMusicList(IStorage storage, int roomID, string musicID)
        {
            await storage.GetRepository<IRoomMusicRepository>()?.Add(roomID, musicID)!;
            storage.Save();
        }

        private RoomMusic? GetRoomMusicInPlaylist(IStorage storage, int roomPlaylistID, string musicID)
        {
            return storage.GetRepository<IRoomPlaylistMusicRepository>().GetRoomPlaylistMusic(roomPlaylistID, musicID)?.roomMusic;
        }

        private RoomMusic GetNextMusicInPlaylist(IStorage storage, int roomPlaylistID, RoomMusic currentMusic, bool isDescending = true)
        {
            return storage.GetRepository<IRoomPlaylistMusicRepository>().GetNextMusic(roomPlaylistID, currentMusic, isDescending);
        }

        private RoomMusic GetPreviousMusicInPlaylist(IStorage storage, int roomPlaylistID, RoomMusic currentMusic, bool isDescending = true)
        {
            return storage.GetRepository<IRoomPlaylistMusicRepository>().GetPreviousMusic(roomPlaylistID, currentMusic, isDescending);
        }
    }
}
