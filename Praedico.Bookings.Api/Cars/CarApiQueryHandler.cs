using Praedico.Bookings.Application.Cars;

//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Cars;

public class CarApiQueryHandler(CarQueryHandler carQueryHandler)
{
    private CarQueryHandler CarQueryHandler{ get; } = carQueryHandler;

    public async Task<IResult> GetAllCars(CancellationToken cancellationToken = default)
    {
        var cars = await CarQueryHandler.GetAllCars(cancellationToken: cancellationToken);
        var result = cars.ToListResponse();
        return Results.Ok(result);
    }

    public async Task<IResult> GetCarByRegistrationNumber(string registrationNumber, CancellationToken cancellationToken = default)
    {
        var car = await CarQueryHandler.GetUniqueAsync(registrationNumber, cancellationToken: cancellationToken);
        return car == null
            ? Results.NotFound($"Car with license plate #{registrationNumber} not found.")
            : Results.Ok(car.ToResponse());
    }
}