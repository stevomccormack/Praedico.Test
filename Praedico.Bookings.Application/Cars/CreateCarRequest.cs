using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Application.Cars;

public record CreateCarRequest
{
    public string CarRegistrationNumber { get; init; } = string.Empty;
    public CarType CarType { get; init; } = CarType.Compact;
}