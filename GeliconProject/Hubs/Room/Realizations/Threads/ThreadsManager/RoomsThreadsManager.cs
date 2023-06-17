using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsManager;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsProvider;

namespace GeliconProject.Hubs.Room.Realizations.Threads.ThreadsManager
{
    public class RoomsThreadsManager : IRoomsThreadsManager
    {
        private Dictionary<string, Action<string, string, object>> actionsWithArgument;
        private Dictionary<string, Action<string, string>> actions;

        public RoomsThreadsManager()
        {
            actionsWithArgument = new Dictionary<string, Action<string, string, object>>();
            actions = new Dictionary<string, Action<string, string>>();
        }

        public void AddActionWithArgument(string name, Action<string, string, object> action)
        {
            actionsWithArgument.Add(name, action);
        }

        public void AddAction(string name, Action<string, string> action)
        {
            actions.Add(name, action);
        }

        public void Invoke(string name, string roomID, string connectionID, object? argument = null)
        {
            if (argument != null)
                actionsWithArgument.GetValueOrDefault(name)?.Invoke(roomID, connectionID, argument);
            else
                actions.GetValueOrDefault(name)?.Invoke(roomID, connectionID);
        }
    }
}
