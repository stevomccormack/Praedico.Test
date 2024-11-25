using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Infrastructure.Data;
using Praedico.Domain;

namespace Praedico.Bookings.Infrastructure.Repositories;

public class CommandRepository<TEntity>(BookingsDbContext dbContext) : ICommandRepository<TEntity>
    where TEntity : class, IEntity
{
    protected readonly BookingsDbContext DbContext = dbContext;
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await DbSet.AddAsync(entity, cancellationToken);
        await DbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        DbSet.Update(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await DbSet.FindAsync(new object[] { id }, cancellationToken);
        if (entity == null) throw new KeyNotFoundException($"Entity with ID {id} not found.");
        DbSet.Remove(entity);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}