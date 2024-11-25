using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Infrastructure.Data;
using Praedico.Domain;

namespace Praedico.Bookings.Infrastructure.Repositories;

public class QueryRepository<TEntity> : IQueryRepository<TEntity> where TEntity : class, IEntity
{
    private readonly DbSet<TEntity> _dbSet;
    protected IQueryable<TEntity> Query;

    public QueryRepository(BookingsDbContext context)
    {
        _dbSet = context.Set<TEntity>();
        Query = _dbSet.AsQueryable();
    }

    public async Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AnyAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<long> CountAsync(Expression<Func<TEntity, bool>>? filter = null, CancellationToken cancellationToken = default)
    {
        return filter == null
            ? await _dbSet.LongCountAsync(cancellationToken)
            : await _dbSet.LongCountAsync(filter, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> ListAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, object>>? orderBy = null,
        bool orderByDescending = false,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        if (filter != null) Query = Query.Where(filter);

        if (orderBy != null)
        {
            Query = orderByDescending
                ? Query.OrderByDescending(orderBy)
                : Query.OrderBy(orderBy);
        }

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            Query = Query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await Query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> SearchAsync(
        string searchText,
        Expression<Func<TEntity, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchText)) throw new ArgumentException("Search text cannot be null or empty.", nameof(searchText));

        if (filter != null) Query = Query.Where(filter);

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            Query = Query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await Query.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<TResult>> QueryRelatedAsync<TResult>(
        Expression<Func<TEntity, bool>> filter,
        Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(filter);
        ArgumentNullException.ThrowIfNull(selector);

        return await _dbSet
            .Where(filter)
            .Select(selector)
            .ToListAsync(cancellationToken);
    }
}
