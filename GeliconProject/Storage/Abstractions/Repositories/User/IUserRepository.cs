namespace GeliconProject.Storage.Repositories.User
{
    public interface IUserRepository : IRepository
    {
        public void Add(Models.User user);

        public Models.User? GetUserByEmail(string email);

        public Models.User GetUserByID(int id);

        public Models.User GetUserByIDWithoutJoins(int id);
    }
}
