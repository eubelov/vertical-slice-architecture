using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;

namespace ProductsApi.DataAccess.EntityService;

public sealed class EntityService : IEntityService, IReadOnlyEntityService
{
    private readonly Context context;

    private readonly ISpecificationEvaluator evaluator = SpecificationEvaluator.Default;

    public EntityService(Context context)
    {
        this.context = context;
    }

    public async Task<TEntity> Create<TEntity>(TEntity entity, CancellationToken token)
        where TEntity : class
    {
        this.context.Set<TEntity>().Add(entity);
        await this.context.SaveChangesAsync(token);

        return entity;
    }

    public async Task<bool> Exists<TEntity>(ISpecification<TEntity> findSpecification, CancellationToken token)
        where TEntity : class
    {
        return await this.ApplySpecification(findSpecification).AnyAsync(token);
    }

    public async Task<TEntity[]> LoadAll<TEntity>(ISpecification<TEntity> findSpecification, CancellationToken token)
        where TEntity : class
    {
        return await this.ApplySpecification(findSpecification).ToArrayAsync(token);
    }

    public async Task Remove<TEntity>(TEntity entity, CancellationToken token)
        where TEntity : class
    {
        this.context.Entry(entity).State = EntityState.Deleted;
        await this.context.SaveChangesAsync(token);
    }

    public async Task RemoveSpec<TEntity>(ISpecification<TEntity> findSpecification, CancellationToken token)
        where TEntity : class
    {
        this.context.RemoveRange(this.ApplySpecification(findSpecification));
        await this.context.SaveChangesAsync(token);
    }

    public async Task<TEntity> Single<TEntity, TSpec>(TSpec specification, CancellationToken token)
        where TEntity : class
        where TSpec : ISpecification<TEntity>, ISingleResultSpecification
    {
        return await this.ApplySpecification(specification).SingleAsync(token);
    }

    public async Task<TEntity?> SingleOrDefault<TEntity, TSpec>(TSpec specification, CancellationToken token)
        where TEntity : class
        where TSpec : ISpecification<TEntity>, ISingleResultSpecification
    {
        return await this.ApplySpecification(specification).SingleOrDefaultAsync(token);
    }

    public async Task<TEntity> Update<TEntity>(TEntity entity, CancellationToken token)
        where TEntity : class
    {
        var entry = this.context.Entry(entity);
        if (entry.State == EntityState.Detached)
        {
            entry.State = EntityState.Modified;
        }

        await this.context.SaveChangesAsync(token);

        return entity;
    }

    private IQueryable<TEntity> ApplySpecification<TEntity>(ISpecification<TEntity> specification, bool evaluateCriteriaOnly = false)
        where TEntity : class
    {
        var set = this.context.Set<TEntity>().AsQueryable();
        return this.evaluator.GetQuery(set, specification, evaluateCriteriaOnly);
    }
}