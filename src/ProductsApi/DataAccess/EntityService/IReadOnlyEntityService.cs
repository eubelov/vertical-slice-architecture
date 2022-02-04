using Ardalis.Specification;

namespace ProductsApi.DataAccess.EntityService;

public interface IReadOnlyEntityService
{
    Task<bool> Exists<TEntity>(ISpecification<TEntity> findSpecification, CancellationToken token)
        where TEntity : class;

    Task<TEntity[]> LoadAll<TEntity>(ISpecification<TEntity> findSpecification, CancellationToken token)
        where TEntity : class;

    Task<TEntity> Single<TEntity, TSpec>(TSpec specification, CancellationToken token)
        where TEntity : class
        where TSpec : ISpecification<TEntity>, ISingleResultSpecification;

    Task<TEntity?> SingleOrDefault<TEntity, TSpec>(TSpec specification, CancellationToken token)
        where TEntity : class
        where TSpec : ISpecification<TEntity>, ISingleResultSpecification;
}