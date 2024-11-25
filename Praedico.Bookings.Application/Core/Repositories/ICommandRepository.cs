using Praedico.Domain;

namespace Praedico.Bookings.Application.Repositories;

public interface ICommandRepository<TEntity> where TEntity : IEntity
{
    Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}