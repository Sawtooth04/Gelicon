namespace GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsManager
{
    public interface IRoomsThreadsManager
    {
        public void AddActionWithArgument(string name, Action<string, string, object> action);

        public void AddAction(string name, Action<string, string> action);

        public void Invoke(string name, string roomID, string connectionID, object? argument = null);
    }
}
