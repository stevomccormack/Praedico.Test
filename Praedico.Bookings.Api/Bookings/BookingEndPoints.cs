using Praedico.Bookings.Application.Bookings;

namespace Praedico.Bookings.Api.Bookings;

public static class BookingEndpoints
{
    public static void UseBookingEndpoints(this WebApplication app)
    {
        var apiGroup = app.MapGroup("/api/bookings").WithTags("Bookings");
        app.MapBookingCommandEndpoints(apiGroup);
        app.MapBookingQueryEndpoints(apiGroup);
    }

    private static void MapBookingQueryEndpoints(this WebApplication app, RouteGroupBuilder apiGroup)
    {
        // rest
        apiGroup.MapGet("/", (BookingApiQueryHandler handler, CancellationToken cancellationToken) => 
                    handler.GetAllBookings(cancellationToken))
                .WithName("GetAllBookings");

        apiGroup.MapGet("/{bookingReference}", (BookingApiQueryHandler handler, string bookingReference, CancellationToken cancellationToken) => 
                handler.GetBookingByReference(bookingReference, cancellationToken))
            .WithName("GetBookingByReference");
        
        // custom
        apiGroup.MapGet("/cartypes/availability", (BookingApiQueryHandler handler, DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken) => 
                handler.CheckCarTypeAvailability(pickupDateTime, returnDateTime, carTypes, cancellationToken))
            .WithName("CheckCarTypeAvailability");
        
        apiGroup.MapGet("/cars/availability", (BookingApiQueryHandler handler, DateTime pickupDateTime, DateTime returnDateTime, string[]? carTypes, CancellationToken cancellationToken) => 
                handler.CheckCarAvailability(pickupDateTime, returnDateTime, carTypes, cancellationToken))
            .WithName("CheckCarAvailability");
    }

    private static void MapBookingCommandEndpoints(this WebApplication app, RouteGroupBuilder apiGroup)
    {
        // rest
        apiGroup.MapPost("/", (BookingApiCommandHandler handler, BookingRequest request, CancellationToken cancellationToken) => 
                handler.CreateBooking(request, cancellationToken))
            .WithName("CreateBooking");

        apiGroup.MapPut("/{bookingReference}", (BookingApiCommandHandler handler, string bookingReference, BookingRequest request, CancellationToken cancellationToken) => 
                handler.UpdateBooking(bookingReference, request, cancellationToken))
            .WithName("UpdateBooking");

        apiGroup.MapDelete("/{bookingReference}", (BookingApiCommandHandler handler, string bookingReference, CancellationToken cancellationToken) => 
                handler.DeleteBooking(bookingReference, cancellationToken))
            .WithName("DeleteBooking");
    }
}