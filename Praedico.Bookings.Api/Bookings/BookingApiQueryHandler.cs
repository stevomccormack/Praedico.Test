using Microsoft.AspNetCore.Mvc;
using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Api.Cars;
//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Bookings;

public class BookingApiQueryHandler(BookingQueryHandler bookingQueryHandler)
{
    private BookingQueryHandler BookingQueryHandler{ get; } = bookingQueryHandler;

    public async Task<IResult> GetAllBookings(CancellationToken cancellationToken = default)
    {
        var bookings = await BookingQueryHandler.GetAllBookings(cancellationToken: cancellationToken);
        var result = bookings.ToResponse();
        return Results.Ok(result);
    }

    public async Task<IResult> GetBookingByReference(string bookingReference, CancellationToken cancellationToken = default)
    {
        var booking = await BookingQueryHandler.GetUniqueAsync(bookingReference, cancellationToken: cancellationToken);
        return booking == null
            ? Results.NotFound($"Booking with reference #{bookingReference} not found.")
            : Results.Ok(booking.ToResponse());
    }
    
    public async Task<IResult> CheckCarTypeAvailability([FromQuery]DateTime pickupDateTime, [FromQuery]DateTime returnDateTime, [FromQuery]string[]? carTypes, CancellationToken cancellationToken = default)
    {
        var availableCarTypes = await BookingQueryHandler.CheckCarTypeAvailability(pickupDateTime, returnDateTime, carTypes,cancellationToken);
        return Results.Ok(availableCarTypes);
    }
    
    public async Task<IResult> CheckCarAvailability([FromQuery]DateTime pickupDateTime, [FromQuery]DateTime returnDateTime, [FromQuery]string[]? carTypes, CancellationToken cancellationToken = default)
    {
        var availableCars = await BookingQueryHandler.CheckCarAvailability(pickupDateTime, returnDateTime, carTypes,cancellationToken);
        var result = availableCars.ToResponse();
        return Results.Ok(availableCars);
    }
}