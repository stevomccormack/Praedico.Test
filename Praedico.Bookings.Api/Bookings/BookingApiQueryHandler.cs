using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Api.Cars;
using Praedico.Bookings.Domain.Cars;

//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Bookings;

public class BookingApiQueryHandler(BookingQueryHandler bookingQueryHandler)
{
    private BookingQueryHandler BookingQueryHandler{ get; } = bookingQueryHandler;

    public async Task<IResult> GetAllBookings(CancellationToken cancellationToken = default)
    {
        var bookings = await BookingQueryHandler.GetAllBookings(cancellationToken: cancellationToken);
        var result = bookings.ToListResponse();
        return Results.Ok(result);
    }

    public async Task<IResult> GetBookingByReference(string bookingReference, CancellationToken cancellationToken = default)
    {
        var booking = await BookingQueryHandler.GetUniqueAsync(bookingReference, cancellationToken: cancellationToken);
        return booking == null
            ? Results.NotFound($"Booking with reference #{bookingReference} not found.")
            : Results.Ok(booking.ToResponse());
    }
    
    public async Task<IResult> CheckCarTypeAvailability(DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken = default)
    {
        var availableCarTypes = await BookingQueryHandler.CheckCarTypeAvailability(pickupDateTime, returnDateTime, carTypes.ToCarTypeArray(),cancellationToken);
        if (!availableCarTypes.Any())
            return Results.NotFound($"No car types: {string.Join(", ", carTypes ?? [])} available between pickup: {pickupDateTime:yy-MMM-dd ddd hh:mm} and return: {returnDateTime:yy-MMM-dd ddd hh:mm} dates.");
       
        var result = availableCarTypes.Select(x => new
        {
            CarType = x.ToString(),
        });
        return Results.Ok(result);
    }
    
    public async Task<IResult> CheckCarAvailability(DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken = default)
    {
        var availableCars = await BookingQueryHandler.CheckCarAvailability(pickupDateTime, returnDateTime, carTypes.ToCarTypeArray(),cancellationToken);
        if (!availableCars.Any())
            return Results.NotFound($"No cars available for car types: {string.Join(", ", carTypes ?? [])} between pickup: {pickupDateTime:yy-MMM-dd ddd hh:mm} and return: {returnDateTime:yy-MMM-dd ddd hh:mm} dates.");

        var result = availableCars.ToListResponse();
        return Results.Ok(result);
    }
}