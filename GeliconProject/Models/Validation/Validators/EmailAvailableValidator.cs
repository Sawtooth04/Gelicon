using GeliconProject.Models.Validation.ValidationDecorators.Abstractions;
using GeliconProject.Storage.Repositories;
using GeliconProject.Storage.Repositories.User;

namespace GeliconProject.Models.Validation.Validators
{
    public class EmailAvailableValidator : IValidator
    {
        private User? component;
        private IUserRepository? repository;

        public EmailAvailableValidator(object component, IRepository repository)
        {
            SetComponents(component, repository);
        }

        public void SetComponent(object component)
        {
            this.component = (User?) component;
        }

        public void SetComponents(object component, IRepository repository)
        {
            this.component = (User?) component;
            this.repository = (IUserRepository) repository;
        }

        public bool Validate()
        {
            if (component != null && repository != null && component.email != null)
            {
                if (repository.GetUserByEmail(component.email) != null)
                    return false;
                return true;
            }
            return false;
        }
    }
}
