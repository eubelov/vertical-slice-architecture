using Ardalis.Specification;

namespace ProductsApi.DataAccess.EntityService;

public interface IEntityService
{
    Task<TEntity> Create<TEntity>(TEntity entity, CancellationToken token)
        where TEntity : class;

    Task<TEntity> Update<TEntity>(TEntity entity, CancellationToken token)
        where TEntity : class;

    Task Remove<TEntity>(TEntity entity, CancellationToken token)
        where TEntity : class;

    Task RemoveSpec<TEntity>(ISpecification<TEntity> findSpecification, CancellationToken token)
        where TEntity : class;
}