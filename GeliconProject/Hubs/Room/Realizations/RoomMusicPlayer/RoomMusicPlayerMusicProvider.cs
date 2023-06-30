using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer;
using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Abstractions.Repositories.RoomMusic;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer
{
    public class RoomMusicPlayerMusicProvider : IRoomMusicPlayerMusicProvider
    {
        private Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>> nextMusicActions;
        private Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>> previousMusicActions;
        private Dictionary<RoomMusicPlayerSource, Func<IStorage, int, string, RoomMusic?>> getMusicActions;
        private Dictionary<RoomMusicPlayerSource, Action<IStorage, int, string>> deleteMusicActions;

        public RoomMusicPlayerMusicProvider()
        {
            nextMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>>();
            previousMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>>();
            getMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, string, RoomMusic?>>();
            deleteMusicActions = new Dictionary<RoomMusicPlayerSource, Action<IStorage, int, string>>();
            ConfigureNextMusicActions();
            ConfigurePreviousMusicActions();
            ConfigureGetMusicActions();
            ConfigureDeleteMusicActions();
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

        private void ConfigureNextMusicActions()
        {
            nextMusicActions.Add(RoomMusicPlayerSource.Music, GetNextMusicInMusicList);
        }

        private void ConfigurePreviousMusicActions()
        {
            previousMusicActions.Add(RoomMusicPlayerSource.Music, GetPreviousMusicInMusicList);
        }

        private void ConfigureGetMusicActions()
        {
            getMusicActions.Add(RoomMusicPlayerSource.Music, GetRoomMusicInMusicList);
        }

        private void ConfigureDeleteMusicActions()
        {
            deleteMusicActions.Add(RoomMusicPlayerSource.Music, DeleteRoomMusicInMusicList);
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
    }
}
