using GeliconProject.Storage.Repositories;

namespace GeliconProject.Storage.Abstractions.Repositories.Color
{
    public interface IColorRepository : IRepository
    {
        public List<Models.Color> All();
    }
}
