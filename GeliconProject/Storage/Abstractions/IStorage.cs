using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions
{
    public interface IStorage
    {
        T GetRepository<T>() where T : IRepository;

        void Save();
    }
}
