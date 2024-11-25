using Praedico.Bookings.Application.Schedules;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories
{
    public class ScheduleCommandRepository : CommandRepository<Schedule>, IScheduleCommandRepository
    {
        public ScheduleCommandRepository(BookingsDbContext dbContext):
            base(dbContext)
        {
            
        }
    }
}