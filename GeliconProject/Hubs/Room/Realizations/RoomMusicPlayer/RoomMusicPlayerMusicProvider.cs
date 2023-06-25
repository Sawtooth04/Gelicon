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

        public RoomMusicPlayerMusicProvider()
        {
            nextMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>>();
            previousMusicActions = new Dictionary<RoomMusicPlayerSource, Func<IStorage, int, RoomMusic, bool, RoomMusic>>();
            ConfigureNextMusicActions();
            ConfigurePreviousMusicActions();
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

        private void ConfigureNextMusicActions()
        {
            nextMusicActions.Add(RoomMusicPlayerSource.Music, GetNextMusicInMusicList);
        }

        private void ConfigurePreviousMusicActions()
        {
            previousMusicActions.Add(RoomMusicPlayerSource.Music, GetPreviousMusicInMusicList);
        }

        private RoomMusic GetNextMusicInMusicList(IStorage storage, int roomID, RoomMusic currentMusic, bool isDescending = true)
        {
            return storage.GetRepository<IRoomMusicRepository>().GetNextMusic(roomID, currentMusic, isDescending);
        }

        private RoomMusic GetPreviousMusicInMusicList(IStorage storage, int roomID, RoomMusic currentMusic, bool isDescending = true)
        {
            return storage.GetRepository<IRoomMusicRepository>().GetPreviousMusic(roomID, currentMusic, isDescending);
        }
    }
}
