using Praedico.Bookings.Application.Cars;

namespace Praedico.Bookings.Api.Cars;

public static class CarEndpoints
{
    public static void UseCarEndpoints(this WebApplication app)
    {
        var apiGroup = app.MapGroup("/api/cars").WithTags("Cars");
        app.MapCarCommandEndpoints(apiGroup);
        app.MapCarQueryEndpoints(apiGroup);
    }

    private static void MapCarQueryEndpoints(this WebApplication app, RouteGroupBuilder apiGroup)
    {
        apiGroup.MapGet("/", (CarApiQueryHandler handler, CancellationToken cancellationToken) => 
                handler.GetAllCars(cancellationToken))
            .WithName("GetAllCars");

        apiGroup.MapGet("/{registrationNumber}", (CarApiQueryHandler handler, string registrationNumber, CancellationToken cancellationToken) => 
                handler.GetCarByRegistrationNumber(registrationNumber, cancellationToken))
            .WithName("GetCarByRegistrationNumber");
    }

    private static void MapCarCommandEndpoints(this WebApplication app, RouteGroupBuilder apiGroup)
    {
        apiGroup.MapPost("/", (CarApiCommandHandler handler, CarRequest request, CancellationToken cancellationToken) => 
                handler.CreateCar(request, cancellationToken))
            .WithName("CreateCar");

        apiGroup.MapPut("/{registrationNumber}", (CarApiCommandHandler handler, string registrationNumber, CarRequest request, CancellationToken cancellationToken) => 
                handler.UpdateCar(registrationNumber, request, cancellationToken))
            .WithName("UpdateCar");

        apiGroup.MapDelete("/{registrationNumber}", (CarApiCommandHandler handler, string registrationNumber, CancellationToken cancellationToken) => 
                handler.DeleteCar(registrationNumber, cancellationToken))
            .WithName("DeleteCar");
    }
}