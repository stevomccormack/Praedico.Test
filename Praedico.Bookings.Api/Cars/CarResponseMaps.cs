using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Api.Cars;

public static class CarResponseMaps
{
    public static IReadOnlyList<CarResponse> ToListResponse(this IReadOnlyList<Car> cars)
    {
        return cars.Select(x => x.ToResponse()).ToList();
    }

    public static CarResponse ToResponse(this Car car)
    {
        return new CarResponse
        {
            //CarId // dont leak domain
            RegistrationNumber = car.RegistrationNumber,
            CarType = car.CarType.ToString(),
            CarStatus = car.Status.ToString()
        };
    }
}