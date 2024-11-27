using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Application.Bookings;

public record CreateBookingRequest
{
    public DateTime PickupDateTime { get; init; }
    public DateTime ReturnDateTime { get; init; }
    
    public string ContactLicenseNumber { get; init; } = string.Empty;
    public string ContactGivenName { get; init; } = string.Empty;
    public string ContactSurname { get; init; } = string.Empty;
    
    public string CarRegistrationNumber { get; init; } = string.Empty;
    public CarType CarType { get; init; } = CarType.Compact;
}