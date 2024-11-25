using Praedico.Bookings.Application.Repositories;

namespace Praedico.Bookings.Application.Schedules;

public interface IScheduleCommandRepository : ICommandRepository<Domain.Schedules.Schedule>
{
        
}