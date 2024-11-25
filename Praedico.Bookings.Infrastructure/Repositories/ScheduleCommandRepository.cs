using Praedico.Bookings.Application.Schedules;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories
{
    public class ScheduleCommandRepository(BookingsDbContext dbContext)
        : CommandRepository<Schedule>(dbContext), IScheduleCommandRepository;
}