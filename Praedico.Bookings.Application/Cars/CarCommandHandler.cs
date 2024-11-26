﻿using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Application.Cars;

public class CarCommandHandler(ICarCommandRepository carCommandRepository)
{
    private ICarCommandRepository CarCommandRepository{ get; } = carCommandRepository;

    public async Task<Car> CreateCar(CreateCarRequest request, CancellationToken cancellationToken = default)
    {
        var carType = CarType.FromName(request.CarType);
        var car = Car.Create(request.CarRegistrationNumber, carType);
        
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