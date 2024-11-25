using Praedico.Bookings.Application.Bookings;

namespace Praedico.Bookings.Api.Bookings;

public static class BookingEndpoints
{
    public static void ConfigureBookingEndpoints(this WebApplication app)
    {
        app.ConfigureBookingCommandEndpoints();
        app.ConfigureBookingQueryEndpoints();
    }

    public static void ConfigureBookingQueryEndpoints(this WebApplication app)
    {
        // rest
        app.MapGet("/api/bookings", (BookingQueryHandler handlers, CancellationToken cancellationToken) => 
                handlers.GetAllBookings(cancellationToken))
            .WithName("GetAllBookings");

        app.MapGet("/api/bookings/{bookingReference}", (BookingQueryHandler handlers, string bookingReference, CancellationToken cancellationToken) => 
                handlers.GetBookingByReference(bookingReference, cancellationToken))
            .WithName("GetBookingByReference");
        
        // custom
        app.MapGet("/api/bookings/availability", (BookingQueryHandler handlers, DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken) => 
                handlers.CheckAvailabilityAsync(pickupDateTime, returnDateTime, carTypes, cancellationToken))
            .WithName("CheckBookingAvailability");
    }
    public static void ConfigureBookingCommandEndpoints(this WebApplication app)
    {
        // rest
        app.MapPost("/api/bookings", (BookingCommandHandler handlers, CreateBookingRequest request, CancellationToken cancellationToken) => 
                handlers.CreateBooking(request, cancellationToken))
            .WithName("CreateBooking");

        app.MapPut("/api/bookings/{bookingReference}", (BookingCommandHandler handlers, string bookingReference, CreateBookingRequest request, CancellationToken cancellationToken) => 
                handlers.UpdateBooking(bookingReference, request, cancellationToken))
            .WithName("UpdateBooking");

        app.MapDelete("/api/bookings/{bookingReference}", (BookingCommandHandler handlers, string bookingReference, CancellationToken cancellationToken) => 
                handlers.DeleteBooking(bookingReference, cancellationToken))
            .WithName("DeleteBooking");
    }
}