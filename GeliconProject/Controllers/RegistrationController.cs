using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GeliconProject.Models;
using GeliconProject.Utils.ApplicationContexts;
using GeliconProject.Repositories;

namespace GeliconProject.Controllers
{
    public class RegistrationController : ControllerBase
    {
        private IRepository repository;

        public RegistrationController(IRepository repository)
        {
            this.repository = repository;
        }

        [HttpPost]
        public async Task Register(string name, string password, string email)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            User user = new User
            {
                name = name,
                email = email
            };
            user.passwordHash = hasher.HashPassword(user, password);
            await repository.AddUser(user);
            repository.SaveChanges();
        }
    }
}