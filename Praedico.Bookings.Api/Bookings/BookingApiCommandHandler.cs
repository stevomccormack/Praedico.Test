using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Application.Contacts;
//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Bookings;

public class BookingApiCommandHandler
{
    private BookingCommandHandler BookingCommandHandler{ get; }
    private CarQueryHandler CarQueryHandler{ get; }
    private ContactQueryHandler ContactQueryHandler{ get; }

    public BookingApiCommandHandler(
        BookingCommandHandler bookingCommandHandler,
        CarQueryHandler carQueryHandler,
        ContactQueryHandler contactQueryHandler)
    {
        BookingCommandHandler = bookingCommandHandler;
        CarQueryHandler = carQueryHandler;
        ContactQueryHandler = contactQueryHandler;
    }

    public async Task<IResult> CreateBooking(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        var booking = await BookingCommandHandler.CreateBooking(request, cancellationToken: cancellationToken);
        var result = booking.ToResponse();
        return Results.Created($"/api/bookings/{booking.BookingReference}", result);
    }

    public IResult UpdateBooking(string bookingReference, CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
        return Results.NoContent();
    }

    public IResult DeleteBooking(string bookingReference, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
        return Results.NoContent();
    }
}