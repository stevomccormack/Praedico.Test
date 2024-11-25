using Praedico.Bookings.Domain.Schedules;

//Mediator - typically use Mediatr for CQRS handling

namespace Praedico.Bookings.Application.Bookings;

public class BookingQueryHandler(IBookingQueryRepository bookingQueryRepository)
{
    private IBookingQueryRepository BookingQueryRepository{ get; } = bookingQueryRepository;

    public async Task<IReadOnlyList<Booking>> GetAllBookings(CancellationToken cancellationToken = default)
    {
        return await BookingQueryRepository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Booking?> GetBookingByReference(string bookingReference, CancellationToken cancellationToken = default)
    {
        return await BookingQueryRepository.GetUniqueAsync(bookingReference, cancellationToken: cancellationToken);
    }
    
    public async Task<Booking?> GetUniqueAsync(string bookingReference, CancellationToken cancellationToken = default)
    {
        return await BookingQueryRepository.GetUniqueAsync(bookingReference, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsUniqueAsync(string bookingReference, CancellationToken cancellationToken = default)
    {
        return await BookingQueryRepository.ExistsUniqueAsync(bookingReference, cancellationToken: cancellationToken);
    }
    
    public async Task<bool> CheckAvailabilityAsync(DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken = default)
    {
        return await BookingQueryRepository.CheckAvailabilityAsync(pickupDateTime, returnDateTime, carTypes,cancellationToken);
    }
}