using Praedico.Bookings.Application.Bookings;

//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Bookings;

public class BookingApiCommandHandler(BookingCommandHandler bookingCommandHandler)
{
    private BookingCommandHandler BookingCommandHandler{ get; } = bookingCommandHandler;

    public async Task<IResult> CreateBooking(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await BookingCommandHandler.CreateBooking(request, cancellationToken: cancellationToken);
        var result = booking.ToResponse();
        return Results.Created($"/api/bookings/{booking.BookingReference}", result);
    }

    public IResult UpdateBooking(string bookingReference, CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        BookingCommandHandler.UpdateBooking(bookingReference, request, cancellationToken: cancellationToken);
        return Results.NoContent();
    }

    public IResult DeleteBooking(string bookingReference, CancellationToken cancellationToken = default)
    {
        BookingCommandHandler.DeleteBooking(bookingReference, cancellationToken: cancellationToken);
        return Results.NoContent();
    }
}