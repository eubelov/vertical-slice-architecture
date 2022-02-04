using Microsoft.EntityFrameworkCore;

using RefactorThis.DataAccess;
using RefactorThis.DataAccess.EntityService;

namespace RefactorThis.UnitTests;

public class UnitTestsBaseWithInMemoryContext : UnitTestBase
{
    public UnitTestsBaseWithInMemoryContext()
    {
        var builder = new DbContextOptionsBuilder<Context>();
        builder.UseInMemoryDatabase($"productsdb-{Guid.NewGuid()}");
        this.Mocker.Use(typeof(DbContextOptions<Context>), builder.Options);
        this.Context = this.Mocker.CreateInstance<Context>();
        this.Mocker.Use(typeof(Context), this.Context);

        var entityService = new EntityService(this.Context);
        this.EntityService = entityService;
        this.ReadOnlyEntityService = entityService;

        this.Mocker.Use(typeof(IEntityService), this.EntityService);
        this.Mocker.Use(typeof(IReadOnlyEntityService), this.ReadOnlyEntityService);
    }

    protected Context Context { get; }

    protected IEntityService EntityService { get; }

    protected IReadOnlyEntityService ReadOnlyEntityService { get; }
}