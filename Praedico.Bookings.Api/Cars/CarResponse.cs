namespace Praedico.Bookings.Api.Cars;

public record CarResponse
{
    public string RegistrationNumber { get; init; } = string.Empty; // Unique
    public string CarType { get; init; } = Domain.Cars.CarType.Compact.ToString();
    public string CarStatus { get; init; } = Domain.Cars.CarStatus.Available.ToString();
}