using GeliconProject.Storage.Abstractions.Context;

namespace GeliconProject.Storage.Repositories
{
    public interface IRepository
    {
        void SetStorageContext(IStorageContext storageContext);
    }
}
