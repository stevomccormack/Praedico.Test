using Praedico.Domain;

namespace Praedico.Bookings.Application.Repositories;

public interface IRepository<TEntity>: ICommandRepository<TEntity>, IQueryRepository<TEntity> where TEntity : IEntity
{
    
}