using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories
{
    public class BookingCommandRepository : CommandRepository<Booking>, IBookingCommandRepository
    {
        public BookingCommandRepository(BookingsDbContext dbContext):
            base(dbContext)
        {
            
        }
    }
}