using Praedico.Bookings.Domain;
using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Application.Cars;

public class CarCommandHandler
{
    private ICarCommandRepository CarCommandRepository{ get; }

    public CarCommandHandler(ICarCommandRepository carCommandRepository)
    {
        CarCommandRepository = carCommandRepository;
    }

    public async Task<Car> CreateCar(CreateCarRequest request, CancellationToken cancellationToken = default)
    {
        var carType = Enumeration.FromName<CarType>(request.CarType);
        var car = Car.Create(carType, request.CarRegistrationNumber);
        
        return await CarCommandRepository.CreateAsync(car, cancellationToken: cancellationToken);
    }

    public void UpdateCar(string bookingReference, CreateCarRequest request, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
    }

    public void DeleteCar(string bookingReference, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
    }
}