using GeliconProject.ApplicationContexts;

namespace GeliconProject.Repositories
{
    public static class RepositoriesExtension
    {
        public static void AddDbRepository(this IServiceCollection collection)
        {
            collection.AddSingleton<IRepository, Repository>();
        }
    }
}
