using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Abstractions.Repositories.Color;

namespace GeliconProject.Storage.Gelicon.Repositories.Color
{
    public class ColorRepository : IColorRepository
    {
        private IStorageContext StorageContext { get; set; }

        public void SetStorageContext(IStorageContext storageContext)
        {
            StorageContext = storageContext;
        }

        public List<Models.Color> All()
        {
            return StorageContext.Colors.ToList();
        }
    }
}
