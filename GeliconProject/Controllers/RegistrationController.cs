using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using GeliconProject.Models;
using GeliconProject.Storage.Abstractions;
using GeliconProject.Storage.Repositories.User;
using GeliconProject.Models.Validation;
using GeliconProject.Models.Validation.Validators;

namespace GeliconProject.Controllers
{
    public class RegistrationController : ControllerBase
    {
        private IStorage storage;

        public RegistrationController(IStorage storage)
        {
            this.storage = storage;
        }

        public bool ValidateUser(User user, string password, IUserRepository repository, out RegistrationValidation registrationValidation)
        {
            RegistrationValidation result = new RegistrationValidation();
            EmailAvailableValidator emailAvailableValidator = new EmailAvailableValidator(user, repository);
            EmailRegexValidator emailRegexValidator = new EmailRegexValidator(user);
            PasswordValidator passwordValidator = new PasswordValidator(password);
            UsernameValidator usernameValidator = new UsernameValidator(user);
            bool status = true;

            status &= result.EmailRegex = emailRegexValidator.Validate();
            status &= result.EmailAvailable = emailAvailableValidator.Validate();
            status &= result.PasswordCorrect = passwordValidator.Validate();
            status &= result.UsernameCorrect = usernameValidator.Validate();

            registrationValidation = result;
            return status;
        }

        [HttpPost]
        public ActionResult Register(string name, string password, string email)
        {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            IUserRepository repository = storage.GetRepository<IUserRepository>();
            RegistrationValidation registrationValidation;
            User user = new User()
            {
                name = name,
                email = email
            };

            user.passwordHash = hasher.HashPassword(user, password);
            if (ValidateUser(user, password, repository, out registrationValidation))
            {
                repository.Add(user);
                storage.Save();
            }
            return Ok(registrationValidation);
        }
    }
}