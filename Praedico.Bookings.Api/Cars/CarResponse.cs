using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Api.Cars;

public record CarResponse
{
    public string RegistrationNumber { get; init; } = string.Empty; // Unique
    public string CarType { get; init; } = Domain.Cars.CarType.Compact.Name;
    public string HireStatus { get; init; } = CarStatus.Available.Name;
}