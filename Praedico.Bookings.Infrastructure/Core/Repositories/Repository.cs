using System.Linq.Expressions;
using Praedico.Bookings.Application.Repositories;
using Praedico.Domain;

namespace Praedico.Bookings.Infrastructure.Repositories;

public class Repository<TEntity>(
    ICommandRepository<TEntity> commandRepository,
    IQueryRepository<TEntity> queryRepository)
    : ICommandRepository<TEntity>, IQueryRepository<TEntity>
    where TEntity : class, IEntity
{
    private ICommandRepository<TEntity> CommandRepository { get; } = commandRepository;
    private IQueryRepository<TEntity> QueryRepository { get; } = queryRepository;

    #region ICommandRepository Implementation

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await CommandRepository.CreateAsync(entity, cancellationToken);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        return await CommandRepository.UpdateAsync(entity, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await CommandRepository.DeleteAsync(id, cancellationToken);
    }

    #endregion

    #region IQueryRepository Implementation

    public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await QueryRepository.GetAsync(id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await QueryRepository.ExistsAsync(id, cancellationToken);
    }

    public async Task<long> CountAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        CancellationToken cancellationToken = default)
    {
        return await QueryRepository.CountAsync(filter, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool orderByDescending = false,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        return await QueryRepository.ListAsync(filter, orderBy, orderByDescending, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> SearchAsync(
        string searchText,
        Expression<Func<TEntity, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        return await QueryRepository.SearchAsync(searchText, filter, pageNumber, pageSize, cancellationToken);
    }

    public async Task<IReadOnlyList<TResult>> QueryRelatedAsync<TResult>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        return await QueryRepository.QueryRelatedAsync(filter, selector, cancellationToken);
    }

    #endregion
}
