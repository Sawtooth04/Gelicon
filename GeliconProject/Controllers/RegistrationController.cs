using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Repositories.User;

namespace GeliconProject.Controllers
{
    public class RegistrationController : ControllerBase
    {
        private IStorage storage;

        public RegistrationController(IStorage storage)
        {
            this.storage = storage;
        }

        [HttpPost]
        public void Register(string name, string password, string email)
        {
            
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            User user = new User()
            {
                name = name,
                email = email
            };
            user.passwordHash = hasher.HashPassword(user, password);
            storage.GetRepository<IUserRepository>()?.Add(user);
            storage.Save();
        }
    }
}