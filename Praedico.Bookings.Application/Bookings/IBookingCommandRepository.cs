using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Application.Bookings;

public interface IBookingCommandRepository : ICommandRepository<Booking>
{
    Task DeleteUniqueAsync(string bookingReference, CancellationToken cancellationToken = default);
}