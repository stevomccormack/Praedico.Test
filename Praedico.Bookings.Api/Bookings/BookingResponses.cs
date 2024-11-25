using EBookingStatus = Praedico.Bookings.Domain.Schedules.BookingStatus;
using ECarType = Praedico.Bookings.Domain.Cars.CarType;
using ECarStatus = Praedico.Bookings.Domain.Cars.CarHireStatus;

namespace Praedico.Bookings.Api.Bookings;

public record BookingResponse
{
    public string BookingReference { get; init; } = string.Empty; // Unique
    public DateTime PickupDateTime { get; init; }
    public DateTime ReturnDateTime { get; init; }
    public string BookingStatus { get; init; } = EBookingStatus.Placed.Name;
    public DateTime? StatusChangedOn { get; init; }
    public DateTime? LastModifiedOn { get; init; }
    public DateTime CreatedOn { get; init; }

    public ContactResponse Contact { get; init; } = new();
    public CarResponse Car { get; init; } = new();
}

public record ContactResponse
{
    public string LicenseNumber { get; init; } = string.Empty; // Unique
    public string GivenName { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; }
}

public record CarResponse
{
    public string RegistrationNumber { get; init; } = string.Empty; // Unique
    public string CarType { get; init; } = ECarType.Compact.Name;
    public string HireStatus { get; init; } = ECarStatus.Available.Name;
}