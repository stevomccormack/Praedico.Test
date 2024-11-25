using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Application.Cars;

public interface ICarQueryRepository : IQueryRepository<Car>
{
    Task<Car?> GetUniqueAsync(string registrationNumber, CancellationToken cancellationToken = default);

    Task<bool> ExistsUniqueAsync(string registrationNumber, CancellationToken cancellationToken = default);
}