using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Api.Contacts;

public static class CarRequestMap
{
    public static Car ToDomain(this CreateCarRequest request)
    {
        var carType = request.CarType;
        var car = Car.Create(request.CarRegistrationNumber, carType);
        
        return car;
    }
}