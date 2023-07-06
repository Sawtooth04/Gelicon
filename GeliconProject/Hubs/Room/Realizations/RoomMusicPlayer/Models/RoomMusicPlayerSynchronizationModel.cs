using GeliconProject.Hubs.Room.Abstractions.RoomMusicPlayer.Models;
using GeliconProject.Utils.MusicPlayerSynchronizationParameters;

namespace GeliconProject.Hubs.Room.Realizations.RoomMusicPlayer.Models
{
    public class RoomMusicPlayerSynchronizationModel : IRoomMusicPlayerSynchronizationModel
    {
        public double GetCurrentTime(IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            List<double> currentTimes = roomMusicPlayerModel.GetClientRoomMusicPlayerModels().ConvertAll(c => c.Value.CurrentTime);
            if (currentTimes.Count > 0)
            {
                currentTimes.Sort();
                return currentTimes[currentTimes.Count / 2];
            }
            return 0;
        }

        public void SynchronizeModels(IRoomMusicPlayerModel roomMusicPlayerModel)
        {
            double currentTime = GetCurrentTime(roomMusicPlayerModel);
            List<KeyValuePair<string, IClientRoomMusicPlayerModel>> clientsModels = roomMusicPlayerModel.GetClientRoomMusicPlayerModels();

            foreach (KeyValuePair<string, IClientRoomMusicPlayerModel> clientModel in clientsModels)
            {
                TryRewind(clientModel.Value, currentTime);
                TryChangeSpeedRatio(clientModel.Value, currentTime);
            }
        }

        private bool TryRewind(IClientRoomMusicPlayerModel clientModel, double currentTime)
        {
            if (CheckForRewind(clientModel, currentTime))
                clientModel.NeedToRewind = true;
            else
                clientModel.NeedToRewind = false;
            return clientModel.NeedToRewind;
        }

        private bool CheckForRewind(IClientRoomMusicPlayerModel clientModel, double currentTime)
        {
            if (Math.Abs(clientModel.CurrentTime - currentTime) > SynchronizationParameters.RewindAbsoluteBorder)
                return true;
            return false;
        }

        private void TryChangeSpeedRatio(IClientRoomMusicPlayerModel clientModel, double currentTime)
        {
            double compareResult = CompareChangeSpeedRatio(clientModel, currentTime);

            if (!clientModel.NeedToRewind)
            {
                if (compareResult < -SynchronizationParameters.MaxSpeedRatioAbsoluteBorder)
                    clientModel.SpeedRatio = SynchronizationParameters.MaxSpeedRatio;
                else if (compareResult > SynchronizationParameters.MaxSpeedRatioAbsoluteBorder)
                    clientModel.SpeedRatio = SynchronizationParameters.MinSpeedRatio;
                else if (compareResult >= -SynchronizationParameters.MaxSpeedRatioAbsoluteBorder && compareResult < -SynchronizationParameters.DefaultSpeedRatioAbsoluteBorder)
                    clientModel.SpeedRatio = SynchronizationParameters.DefaultSpeedRatio - SynchronizationParameters.SpeedRatioMultiplier * compareResult;
                else if (compareResult <= SynchronizationParameters.MaxSpeedRatioAbsoluteBorder && compareResult > SynchronizationParameters.DefaultSpeedRatioAbsoluteBorder)
                    clientModel.SpeedRatio = SynchronizationParameters.DefaultSpeedRatio + SynchronizationParameters.SpeedRatioMultiplier * compareResult;
                else
                    clientModel.SpeedRatio = SynchronizationParameters.DefaultSpeedRatio;
            }
        }

        private double CompareChangeSpeedRatio(IClientRoomMusicPlayerModel clientModel, double currentTime)
        {
            return clientModel.CurrentTime - currentTime;
        }
    }
}
