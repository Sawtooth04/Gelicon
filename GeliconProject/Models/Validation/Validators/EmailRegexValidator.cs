using GeliconProject.Models.Validation.ValidationDecorators.Abstractions;
using GeliconProject.Storage.Repositories;
using System.Text.RegularExpressions;

namespace GeliconProject.Models.Validation.Validators
{
    public class EmailRegexValidator : IValidator
    {
        private User? component;

        public EmailRegexValidator(object component)
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
            if (component != null && component.email != null)
            {
                Regex regex = new Regex(@"\S+@\S+\.\S+$");
                return regex.IsMatch(component.email);
            }
            return false;
        }
    }
}
