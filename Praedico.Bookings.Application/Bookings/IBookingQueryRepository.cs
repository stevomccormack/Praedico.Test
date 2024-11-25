using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Application.Bookings;

public interface IBookingQueryRepository : IQueryRepository<Booking>
{
    Task<Booking?> GetUniqueAsync(string bookingReference, CancellationToken cancellationToken = default);

    Task<bool> ExistsUniqueAsync(string bookingReference, CancellationToken cancellationToken = default);

    Task<bool> CheckAvailabilityAsync(DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken = default);
}