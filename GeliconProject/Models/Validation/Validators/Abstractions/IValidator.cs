using GeliconProject.Storage.Repositories;

namespace GeliconProject.Models.Validation.ValidationDecorators.Abstractions
{
    public interface IValidator : IValidateable
    {
        public void SetComponent(object component);

        public void SetComponents(object component, IRepository repository);
    }
}
