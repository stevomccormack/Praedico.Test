using System.Linq.Expressions;
using Praedico.Domain;

namespace Praedico.Bookings.Application.Repositories;

public interface IQueryRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    Task<long> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool orderByDescending = false,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<TEntity>> SearchAsync(
        string searchText,
        Expression<Func<TEntity, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyList<TResult>> QueryRelatedAsync<TResult>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default
    );
}