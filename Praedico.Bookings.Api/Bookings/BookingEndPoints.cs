using Praedico.Bookings.Application.Bookings;

namespace Praedico.Bookings.Api.Bookings;

public static class BookingEndpoints
{
    public static void UseBookingEndpoints(this WebApplication app)
    {
        app.MapBookingCommandEndpoints();
        app.MapBookingQueryEndpoints();
    }

    private static void MapBookingQueryEndpoints(this WebApplication app)
    {
        // rest
        app.MapGet("/api/bookings", (BookingApiQueryHandler handler, CancellationToken cancellationToken) => 
                handler.GetAllBookings(cancellationToken))
            .WithName("GetAllBookings");

        app.MapGet("/api/bookings/{bookingReference}", (BookingApiQueryHandler handler, string bookingReference, CancellationToken cancellationToken) => 
                handler.GetBookingByReference(bookingReference, cancellationToken))
            .WithName("GetBookingByReference");
        
        // custom
        app.MapGet("/api/bookings/cartypes/availability", (BookingApiQueryHandler handler, DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken) => 
                handler.CheckCarTypeAvailability(pickupDateTime, returnDateTime, carTypes, cancellationToken))
            .WithName("CheckCarTypeAvailability");
        
        app.MapGet("/api/bookings/cars/availability", (BookingApiQueryHandler handler, DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken) => 
                handler.CheckCarAvailability(pickupDateTime, returnDateTime, carTypes, cancellationToken))
            .WithName("CheckCarAvailability");
    }

    private static void MapBookingCommandEndpoints(this WebApplication app)
    {
        // rest
        app.MapPost("/api/bookings", (BookingApiCommandHandler handler, CreateBookingRequest request, CancellationToken cancellationToken) => 
                handler.CreateBooking(request, cancellationToken))
            .WithName("CreateBooking");

        app.MapPut("/api/bookings/{bookingReference}", (BookingApiCommandHandler handler, string bookingReference, CreateBookingRequest request, CancellationToken cancellationToken) => 
                handler.UpdateBooking(bookingReference, request, cancellationToken))
            .WithName("UpdateBooking");

        app.MapDelete("/api/bookings/{bookingReference}", (BookingApiCommandHandler handler, string bookingReference, CancellationToken cancellationToken) => 
                handler.DeleteBooking(bookingReference, cancellationToken))
            .WithName("DeleteBooking");
    }
}