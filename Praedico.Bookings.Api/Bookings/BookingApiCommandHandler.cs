using Praedico.Bookings.Application.Bookings;

//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Bookings;

public class BookingApiCommandHandler(BookingCommandHandler bookingCommandHandler)
{
    private BookingCommandHandler BookingCommandHandler{ get; } = bookingCommandHandler;

    public async Task<IResult> CreateBooking(BookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await BookingCommandHandler.CreateBooking(request, cancellationToken: cancellationToken);
        var result = booking.ToResponse();
        return Results.Created($"/api/bookings/{booking.BookingReference}", result);
    }

    public async Task<IResult> UpdateBooking(string bookingReference, BookingRequest request, CancellationToken cancellationToken = default)
    {
        await BookingCommandHandler.UpdateBooking(bookingReference, request, cancellationToken: cancellationToken);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteBooking(string bookingReference, CancellationToken cancellationToken = default)
    {
        await BookingCommandHandler.DeleteBooking(bookingReference, cancellationToken: cancellationToken);
        return Results.NoContent();
    }
}