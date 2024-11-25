using Praedico.Bookings.Domain.Cars;
using Praedico.Guards;

namespace Praedico.Bookings.Domain.Schedules;

public class Schedule: AggregateRoot
{
    public string LocationCode { get; }
    public DateTime CreatedOn { get; } = DateTime.UtcNow;

    private readonly List<Booking> _bookings = new();
    public IReadOnlyList<Booking> Bookings => _bookings.AsReadOnly();

    private Schedule(Guid id, string locationCode) : base(id)
    {
        LocationCode = locationCode;
    }

    public static Schedule Create(string locationCode)
    {
        Guard.Against.NullOrWhiteSpace(locationCode, nameof(locationCode));
        
        return new Schedule(Guid.NewGuid(), locationCode);
    }
    
    public void PlaceBooking(Booking booking)
    {
        Guard.Against.Null(booking, nameof(booking));
        Guard.Against.InvalidLocation(LocationCode);
        Guard.Against.InactiveCar(booking.Car);
        Guard.Against.ContactCollisions(booking, Bookings);
        Guard.Against.CarHireCollisions(booking, Bookings);

        _bookings.Add(booking);

        //RaiseDomainEvent(new BookingPlaced(booking, ...));
    }

    public void ConfirmBooking(Booking booking)
    {
        Guard.Against.Null(booking, nameof(booking));
        Guard.Against.InvalidLocation(LocationCode);
        Guard.Against.InactiveCar(booking.Car);

        booking.Confirm();

        //RaiseDomainEvent(new BookingConfirmed(booking, ...));
    }

    public void RescheduleBooking(Booking booking, DateTimeRange rescheduledTimeRange)
    {
        Guard.Against.Null(booking, nameof(booking));
        Guard.Against.Null(rescheduledTimeRange, nameof(rescheduledTimeRange));
        Guard.Against.InactiveCar(booking.Car);
        Guard.Against.Collisions(booking, rescheduledTimeRange, Bookings);

        booking.ReSchedule(rescheduledTimeRange);

        //RaiseDomainEvent(new BookingRescheduled(booking, ...));
    }

    public void CancelBooking(Booking booking)
    {
        Guard.Against.Null(booking, nameof(booking));

        booking.Cancel();

        //RaiseDomainEvent(new BookingCancelled(booking, ...));
    }

    public void AbandonBooking(Booking booking)
    {
        Guard.Against.Null(booking, nameof(booking));

        booking.Abandon();

        //RaiseDomainEvent(new BookingAbandoned(booking, ...));
    }
}