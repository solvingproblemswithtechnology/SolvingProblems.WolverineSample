using SolvingProblems.WolverineSample.Domain.Abstract;
using System.Linq.Expressions;

namespace SolvingProblems.WolverineSample.Infrastructure.Data.Shared;

public class EntityFrameworkRepository<TEntity> : EntityFrameworkRepository<TEntity, GuidEntityId>, IRepository<TEntity>
    where TEntity : AggregateRoot<GuidEntityId>
{
    public EntityFrameworkRepository(SmartDbContext context) : base(context)
    {
    }
}

public class EntityFrameworkRepository<TEntity, TIdentifier> : IRepository<TEntity, TIdentifier>
    where TEntity : AggregateRoot<TIdentifier>
    where TIdentifier : EntityId
{
    private readonly SmartDbContext context;

    public IUnitOfWork UnitOfWork => this.context;

    public EntityFrameworkRepository(SmartDbContext context)
    {
        this.context = context;
    }

    public ValueTask<TEntity?> FindAsync(TIdentifier identifier) => this.context.FindAsync<TEntity>();

    public void Add(TEntity entity) => this.context.Set<TEntity>().Add(entity);
    public void AddRange(IEnumerable<TEntity> entities) => this.context.Set<TEntity>().AddRange(entities);

    public void Update(TEntity entity) => this.context.Set<TEntity>().Update(entity);
    public void UpdateRange(IEnumerable<TEntity> entities) => this.context.Set<TEntity>().UpdateRange(entities);

    public void Delete(TEntity entity) => this.context.Set<TEntity>().Remove(entity);
    public void DeleteRange(IEnumerable<TEntity> entities) => this.context.Set<TEntity>().RemoveRange(entities);

    public IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> filter) => this.context.Set<TEntity>().Where(filter);
}
