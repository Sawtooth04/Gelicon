using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Gelicon.Context;
using GeliconProject.Storage.Repositories;
using System.Reflection;

namespace GeliconProject.Storage.Gelicon
{
    public class DbStorage : IStorage
    {
        public IStorageContext StorageContext { get; private set; }

        public DbStorage(IStorageContext storageContext)
        {
            StorageContext = storageContext;
        }

        public T? GetRepository<T>() where T : IRepository
        {
            foreach (Type type in this.GetType().GetTypeInfo().Assembly.GetTypes())
            {
                if (typeof(T).GetTypeInfo().IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
                {
                    T repository = (T) Activator.CreateInstance(type)!;
                    repository.SetStorageContext(StorageContext);
                    return repository;
                }
            }
            return default(T);
        }

        public void Save()
        {
            StorageContext.SaveChanges();
        }
    }
}
