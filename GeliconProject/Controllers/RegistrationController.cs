using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GeliconProject.Models;
using GeliconProject.Utils.ApplicationContexts;

namespace GeliconProject.Controllers
{
    public class RegistrationController : ControllerBase
    {
        private ApplicationContext context;

        public RegistrationController(ApplicationContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task Register(string name, string password, string email)
        {
            User user = new User();
            user.name = name;
            user.email = email;
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            user.passwordHash = hasher.HashPassword(user, password);
            await context.Users.AddAsync(user);
            context.SaveChanges();
        }

    }
}