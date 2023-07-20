using System.Linq.Expressions;

namespace SolvingProblems.WolverineSample.Domain.Abstract;

public interface IRepository<TEntity> : IRepository<TEntity, GuidEntityId>
    where TEntity : AggregateRoot<GuidEntityId>
{ }

public interface IRepository<TEntity, TIdentifier>
    where TEntity : AggregateRoot<TIdentifier>
    where TIdentifier : EntityId
{
    IUnitOfWork UnitOfWork { get; }

    ValueTask<TEntity?> FindAsync(TIdentifier identifier);

    void Add(TEntity entity);
    void AddRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);

    IQueryable<TEntity> GetBy(Expression<Func<TEntity, bool>> filter);
}
