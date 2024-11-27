using Praedico.Bookings.Application.Cars;

//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Cars;

public class CarApiCommandHandler(CarCommandHandler carCommandHandler)
{
    private CarCommandHandler CarCommandHandler{ get; } = carCommandHandler;

    public async Task<IResult> CreateCar(CarRequest request, CancellationToken cancellationToken = default)
    {
        var car = await CarCommandHandler.CreateCar(request, cancellationToken: cancellationToken);
        var result = car.ToResponse();
        return Results.Created($"/api/cars/{car.RegistrationNumber}", result);
    }

    public async Task<IResult> UpdateCar(string carReference, CarRequest request, CancellationToken cancellationToken = default)
    {
        await CarCommandHandler.UpdateCar(carReference, request, cancellationToken: cancellationToken);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteCar(string carReference, CancellationToken cancellationToken = default)
    {
        await CarCommandHandler.DeleteCar(carReference, cancellationToken: cancellationToken);
        return Results.NoContent();
    }
}