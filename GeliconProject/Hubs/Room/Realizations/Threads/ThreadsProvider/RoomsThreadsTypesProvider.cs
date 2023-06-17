using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsContainer;

namespace GeliconProject.Hubs.Room.Realizations.Threads.ThreadsContainer
{
    public class RoomsThreadsTypesProvider : IRoomsThreadsTypesProvider
    {
        private Dictionary<Type, Type> threadsTypesDictionary;

        public RoomsThreadsTypesProvider()
        {
            threadsTypesDictionary = new Dictionary<Type, Type>();
        }

        public void Add(Type key, Type value)
        {
            threadsTypesDictionary.Add(key, value);
        }

        public T TryCreateInstance<T>()
        {
            T? result = (T?) Activator.CreateInstance(TryGetType(typeof(T)));
            if (result != null)
                return result;
            else
                throw new Exception("Unable to create instance!");
        }

        public Type TryGetType(Type key)
        {
            Type? result;
            if (threadsTypesDictionary.TryGetValue(key, out result))
                return result;
            else
                throw new Exception("Unable to find type!");
        }
    }
}
