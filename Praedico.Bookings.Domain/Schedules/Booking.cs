using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Guards;

namespace Praedico.Bookings.Domain.Schedules;


public class Booking: Entity
{
    private const int MinBookingTimeInHours = 1;
    private const int MaxBookingTimeInDays = 180;
    public string BookingReference { get; } = GenerateBookingReference(); //unique
    public DateTime PickupDateTime { get; private set; }
    public DateTime ReturnDateTime { get; private set; }
    public DateTimeRange TimeRange => DateTimeRange.Create(PickupDateTime, ReturnDateTime);
    public BookingStatus Status { get; private set; } = BookingStatus.Placed;
    public DateTime? StatusChangedOn { get; private set; }
    public DateTime CreatedOn { get; } = DateTime.UtcNow;
    public DateTime? LastModifiedOn { get; private set; }
    public Contact Contact { get; private set; }
    public Car Car { get; private set; }

    private Booking(Guid id) : base(id)
    {
        
    }

    public static Booking Create(Contact contact, Car car, DateTime pickupDateTime, DateTime returnDateTime)
    {
        Guard.Against.Null(contact, nameof(contact));
        Guard.Against.Null(car, nameof(car));
        Guard.Against.Null(pickupDateTime, nameof(pickupDateTime));
        Guard.Against.Null(returnDateTime, nameof(returnDateTime));
        
        var timeRange = DateTimeRange.Create(pickupDateTime, returnDateTime);
        Guard.Against.MinAllowedBookingTime(timeRange);
        Guard.Against.MaxAllowedBookingTime(timeRange, MaxBookingTimeInDays);
        Guard.Against.HistoricalBookingTime(timeRange);

        return new Booking(Guid.NewGuid())
        {
            PickupDateTime = pickupDateTime,
            ReturnDateTime = returnDateTime,
            Contact = contact,
            Car = car
        };
    }

    public void Confirm()
    {
        Guard.Against.InactiveContact(Contact);
        Guard.Against.InactiveCar(Car);
        Guard.Against.InvalidChangeBookingStatus(Status, BookingStatus.Confirmed);
        Guard.Against.ChangeHistoricalBooking(this);
        Guard.Against.ChangeTerminatedBooking(this);

        ChangeStatus(BookingStatus.Confirmed);
    }

    public void ReSchedule(DateTime pickupDateTime, DateTime returnDateTime)
    {
        Guard.Against.Null(pickupDateTime, nameof(pickupDateTime));
        Guard.Against.Null(returnDateTime, nameof(returnDateTime));
        Guard.Against.InactiveContact(Contact);
        Guard.Against.InactiveCar(Car);

        var timeRange = DateTimeRange.Create(pickupDateTime, returnDateTime);
        Guard.Against.MinAllowedBookingTime(timeRange, MinBookingTimeInHours);
        Guard.Against.MaxAllowedBookingTime(timeRange, MaxBookingTimeInDays);
        Guard.Against.ChangeHistoricalBooking(this);
        Guard.Against.ChangeTerminatedBooking(this);

        if (!PickupDateTime.Equals(pickupDateTime))
            PickupDateTime = pickupDateTime;
        if (!ReturnDateTime.Equals(returnDateTime))
            ReturnDateTime = returnDateTime;

        ChangeStatus(BookingStatus.Confirmed);
    }

    public void Cancel()
    {
        Guard.Against.InvalidChangeBookingStatus(Status, BookingStatus.Cancelled);
        Guard.Against.ChangeHistoricalBooking(this);

        ChangeStatus(BookingStatus.Cancelled);
    }

    public void Abandon()
    {
        Guard.Against.InvalidChangeBookingStatus(Status, BookingStatus.Abandoned);
        Guard.Against.ChangeHistoricalBooking(this);

        ChangeStatus(BookingStatus.Abandoned);
    }

    public void ChangeContact(Contact contact)
    {
        Guard.Against.Null(contact, nameof(contact));
        Guard.Against.InactiveContact(contact);
        Guard.Against.ChangeHistoricalBooking(this);
        Guard.Against.ChangeTerminatedBooking(this);

        if (!contact.Equals(Contact))
            Contact = contact;
        LastModifiedOn = DateTime.UtcNow;
    }

    public void ChangeCar(Car car)
    {
        Guard.Against.Null(car, nameof(car));
        Guard.Against.InactiveCar(car);
        Guard.Against.ChangeHistoricalBooking(this);
        Guard.Against.ChangeTerminatedBooking(this);

        if (!car.Equals(Car))
            Car = car;
        LastModifiedOn = DateTime.UtcNow;
    }

    private void ChangeStatus(BookingStatus status)
    {
        if (!status.Equals(Status))
            Status = status;
        
        StatusChangedOn = DateTime.UtcNow;
        LastModifiedOn = DateTime.UtcNow;
    }

    private static string GenerateBookingReference()
    {
        var ticks = new DateTime(DateTime.Now.AddYears(-2).Year, DateTime.Now.Month, DateTime.Now.Day).Ticks;
        return $"BK-{(DateTime.Now.Ticks - ticks):x}".ToUpper();
    }
}