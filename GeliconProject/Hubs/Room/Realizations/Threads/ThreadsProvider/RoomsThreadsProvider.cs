using GeliconProject.Hubs.Room.Abstractions.Threads;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsContainer;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsManager;
using GeliconProject.Hubs.Room.Abstractions.Threads.ThreadsProvider;
using GeliconProject.Hubs.Room.Realizations.Threads.ThreadsContainer;
using Microsoft.AspNetCore.SignalR;

namespace GeliconProject.Hubs.Room.Realizations.Threads.ThreadsProvider
{
    public class RoomsThreadsProvider : IRoomsThreadsProvider
    {
        private List<IRoomObserverThread> roomObservers;
        private IRoomsThreadsTypesProvider typesProvider;

        public RoomsThreadsProvider(IRoomsThreadsManager roomsThreadsManager)
        {
            roomObservers = new List<IRoomObserverThread>();
            typesProvider = new RoomsThreadsTypesProvider();
            Configuring();
            ActionsRegistration(roomsThreadsManager);
        }

        private void Configuring()
        {
            typesProvider.Add(typeof(IRoomObserverThread), typeof(RoomObserverThread));
            typesProvider.Add(typeof(IClientObserverThread), typeof(ClientObserverThread));
        }

        private void ActionsRegistration(IRoomsThreadsManager roomsThreadsManager)
        {
            roomsThreadsManager.AddAction("GetCurrentMusic", GetCurrentMusic);
        }

        public IRoomObserverThread CreateRoomObserverThread(string roomID, IHubCallerClients clients)
        {
            IRoomObserverThread? roomObserverThread;

            if ((roomObserverThread = FindRoomObserverThread(roomID)) == null)
            {
                roomObserverThread = typesProvider.TryCreateInstance<IRoomObserverThread>();
                roomObserverThread.RoomID = roomID;
                roomObserverThread.AddEmptyRoomHandler(EmptyRoomHandler);
                roomObservers.Add(roomObserverThread);
                roomObserverThread.Start();
            }
            return roomObserverThread;
        }

        public IRoomObserverThread? FindRoomObserverThread(string roomID)
        {
            return roomObservers.Find(r => r.RoomID == roomID);
        }

        private void EmptyRoomHandler(IRoomObserverThread thread)
        {
            roomObservers.Remove(thread);
        }

        private void GetCurrentMusic(string roomID, string connectionID)
        {
            FindRoomObserverThread(roomID)?.SetClientMusic(connectionID);
        }
    }
}
