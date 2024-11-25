using Praedico.Bookings.Application.Repositories;

namespace Praedico.Bookings.Application.Schedules;

public interface IScheduleQueryRepository : IQueryRepository<Domain.Schedules.Schedule>
{
    Task<Domain.Schedules.Schedule?> GetUniqueAsync(string locationCode, CancellationToken cancellationToken = default);

    Task<bool> ExistsUniqueAsync(string locationCode, CancellationToken cancellationToken = default);
}