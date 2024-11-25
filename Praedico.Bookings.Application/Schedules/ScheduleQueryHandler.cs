using Praedico.Bookings.Domain.Schedules;

//Mediator - typically use Mediatr for CQRS handling

namespace Praedico.Bookings.Application.Schedules;

public class ScheduleQueryHandler
{
    private IScheduleQueryRepository ScheduleQueryRepository{ get; }

    public ScheduleQueryHandler(IScheduleQueryRepository carQueryRepository)
    {
        ScheduleQueryRepository = carQueryRepository;
    }
    
    public async Task<IReadOnlyList<Schedule>> GetAllSchedules(CancellationToken cancellationToken = default)
    {
        return await ScheduleQueryRepository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Schedule?> GetScheduleByReference(string bookingReference, CancellationToken cancellationToken = default)
    {
        return await ScheduleQueryRepository.GetUniqueAsync(bookingReference, cancellationToken: cancellationToken);
    }
}