using GeliconProject.Models.Validation.ValidationDecorators.Abstractions;
using GeliconProject.Storage.Repositories;
using System.Text.RegularExpressions;

namespace GeliconProject.Models.Validation.Validators
{
    public class UsernameValidator : IValidator
    {
        private User? component;

        public UsernameValidator(object component)
        {
            SetComponent(component);
        }

        public void SetComponent(object component)
        {
            this.component = (User?) component;
        }

        public void SetComponents(object component, IRepository repository)
        {
            throw new NotImplementedException();
        }

        public bool Validate()
        {
            if (component != null && component.name != null)
            {
                Regex regex = new Regex(@"\S{4,}");
                return regex.IsMatch(component.name);
            }
            return false;
        }
    }
}
