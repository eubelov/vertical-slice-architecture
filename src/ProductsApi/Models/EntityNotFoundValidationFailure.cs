using FluentValidation.Results;

namespace ProductsApi.Models;

public sealed class EntityNotFoundValidationFailure : ValidationFailure
{
    public EntityNotFoundValidationFailure(string entityName, Guid id)
        : base(entityName, $"Entity '{entityName}' with ID '{id}' does not exist.")
    {
    }
}