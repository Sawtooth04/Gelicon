namespace GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsContainer
{
    public interface IRoomsThreadsTypesProvider
    {
        public Type TryGetType(Type key);

        public void Add(Type key, Type value);

        public T TryCreateInstance<T>();
    }
}
