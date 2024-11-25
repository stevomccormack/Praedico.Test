using Praedico.Bookings.Application.Bookings;
//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Bookings;

public class BookingApiQueryHandler(BookingQueryHandler bookingQueryHandler)
{
    private BookingQueryHandler BookingQueryHandler{ get; } = bookingQueryHandler;

    public async Task<IResult> GetAllBookings(CancellationToken cancellationToken = default)
    {
        var bookings = await BookingQueryHandler.GetAllBookings(cancellationToken: cancellationToken);
        var response = bookings.ToResponse();
        return Results.Ok(response);
    }

    public async Task<IResult> GetBookingByReference(string bookingReference, CancellationToken cancellationToken = default)
    {
        var booking = await BookingQueryHandler.GetUniqueAsync(bookingReference, cancellationToken: cancellationToken);
        return booking == null
            ? Results.NotFound($"Booking with reference #{bookingReference} not found.")
            : Results.Ok(booking.ToResponse());
    }
    
    public async Task<IResult> CheckAvailabilityAsync(DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken = default)
    {
        var available = await BookingQueryHandler.CheckAvailabilityAsync(pickupDateTime, returnDateTime, carTypes,cancellationToken);
        return Results.Ok(available);
    }
}