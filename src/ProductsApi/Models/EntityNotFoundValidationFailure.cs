using FluentValidation.Results;

namespace RefactorThis.Models;

public sealed class EntityNotFoundValidationFailure : ValidationFailure
{
    public EntityNotFoundValidationFailure(string entityName, Guid id)
        : base(entityName, $"Entity '{entityName}' with ID '{id}' does not exist.")
    {
    }
}