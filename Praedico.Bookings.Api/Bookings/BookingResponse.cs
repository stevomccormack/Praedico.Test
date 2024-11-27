using Praedico.Bookings.Api.Cars;
using Praedico.Bookings.Api.Contacts;
using EBookingStatus = Praedico.Bookings.Domain.Schedules.BookingStatus;

namespace Praedico.Bookings.Api.Bookings;

public record BookingResponse
{
    public string BookingReference { get; init; } = string.Empty; // Unique
    public DateTime PickupDateTime { get; init; }
    public DateTime ReturnDateTime { get; init; }
    public string BookingStatus { get; init; } = EBookingStatus.Placed.ToString();
    public DateTime? StatusChangedOn { get; init; }
    public DateTime? LastModifiedOn { get; init; }
    public DateTime CreatedOn { get; init; }

    public ContactResponse Contact { get; init; } = new();
    public CarResponse Car { get; init; } = new();
}