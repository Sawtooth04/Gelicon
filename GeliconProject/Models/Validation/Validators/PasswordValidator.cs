using GeliconProject.Models.Validation.ValidationDecorators.Abstractions;
using GeliconProject.Storage.Repositories;
using System.Text.RegularExpressions;

namespace GeliconProject.Models.Validation.Validators
{
    public class PasswordValidator : IValidator
    {
        private string? component;

        public PasswordValidator(object component)
        {
            SetComponent(component);
        }

        public void SetComponent(object component)
        {
            this.component = (string?)component;
        }

        public void SetComponents(object component, IRepository repository)
        {
            throw new NotImplementedException();
        }

        public bool Validate()
        {
            if (component != null)
            {
                Regex regex = new Regex(@"\S{4,}");
                return regex.IsMatch(component);
            }
            return false;
        }
    }
}
