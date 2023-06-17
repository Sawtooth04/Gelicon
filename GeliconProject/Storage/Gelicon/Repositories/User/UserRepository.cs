using GeliconProject.Storage.Abstractions.Context;
using GeliconProject.Storage.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace GeliconProject.Storage.Gelicon.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private IStorageContext StorageContext { get; set; }

        public void SetStorageContext(IStorageContext storageContext)
        {
            StorageContext = storageContext;
        }

        public void Add(Models.User user)
        {
            StorageContext.Users.AddAsync(user);
        }

        public Models.User GetUserByEmail(string email)
        {
            return StorageContext.Users.Where(u => u.email == email).Single();
        }

        public Models.User GetUserByID(int id)
        {
            return StorageContext.Users.Where(u => u.userID == id).Include(u => u.rooms)!.ThenInclude(r => r.owner).Single();
        }

        public Models.User GetUserByIDWithoutJoins(int id)
        {
            return StorageContext.Users.Where(u => u.userID == id).Single();
        }
    }
}
