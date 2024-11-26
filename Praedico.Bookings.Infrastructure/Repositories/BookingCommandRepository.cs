using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;
using Praedico.Exceptions;

namespace Praedico.Bookings.Infrastructure.Repositories
{
    public class BookingCommandRepository(BookingsDbContext dbContext)
        : CommandRepository<Booking>(dbContext), IBookingCommandRepository
    {
        public async Task DeleteUniqueAsync(string bookingReference, CancellationToken cancellationToken = default)
        {
            var booking = await DbSet.FirstOrDefaultAsync(x => x.BookingReference == bookingReference, cancellationToken: cancellationToken);
            if (booking == null)
                throw new NotFoundException($"Booking not found with booking reference: {bookingReference}");
            
            DbContext.Bookings.Remove(booking);
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}