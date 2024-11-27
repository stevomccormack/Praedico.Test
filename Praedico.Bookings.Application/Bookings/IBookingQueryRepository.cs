using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Application.Bookings;

public interface IBookingQueryRepository : IQueryRepository<Booking>
{
    Task<Booking?> GetUniqueAsync(string bookingReference, CancellationToken cancellationToken = default);

    Task<bool> ExistsUniqueAsync(string bookingReference, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CarType>> CheckCarTypeAvailability(DateTime pickupDateTime, DateTime returnDateTime, CarType[]? carTypes, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Car>> CheckCarAvailability(DateTime pickupDateTime, DateTime returnDateTime, CarType[]? carTypes, CancellationToken cancellationToken = default);
}