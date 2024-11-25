namespace Praedico.Bookings.Application.Cars;

public record CreateCarRequest
{
    public string CarRegistrationNumber { get; init; } = string.Empty;
    public string CarType { get; init; } = string.Empty;
}