using System.Text;
using Praedico.Exceptions;
using Praedico.Guards;

namespace Praedico.Bookings.Domain.Schedules;

public static class BookingGuards
{

    public static void InvalidLocation(this IGuardClause guardClause, string locationCode)
    {
        guardClause.Null(locationCode, nameof(locationCode));
        
        //@TODO: Location would be an entity typeof(Location),
        //@TODO: Validate location open hours, active
    }
    public static void MinAllowedBookingTime(this IGuardClause guardClause, DateTimeRange timeRange, int minTimeInHours = 1)
    {
        guardClause.Null(timeRange, nameof(timeRange));
        if ((timeRange.End - timeRange.Start).TotalHours < minTimeInHours)
            throw new BusinessException($"The booking time range must be at least {minTimeInHours} hours long.", 
                "INVALID_MIN_BOOKING_TIME");
    }

    public static void MaxAllowedBookingTime(this IGuardClause guardClause, DateTimeRange timeRange, int maxDays = 180)
    {
        guardClause.Null(timeRange, nameof(timeRange));
        if ((timeRange.End - timeRange.Start).TotalDays > maxDays)
            throw new BusinessException($"The booking time range cannot exceed {maxDays} days.", 
                $"EXCEEDED_MAX_ALLOWED_BOOKING_TIME");
    }

    public static void HistoricalBookingTime(this IGuardClause guardClause, DateTimeRange timeRange)
    {
        guardClause.Null(timeRange, nameof(timeRange));
        if (timeRange.End < DateTime.UtcNow)
            throw new BusinessException("The booking time range cannot be in the past.", 
                "INVALID_HISTORICAL_BOOKING");
    }

    public static void ChangeHistoricalBooking(this IGuardClause guardClause, Booking booking)
    {
        guardClause.Null(booking, nameof(booking));
        if (booking.TimeRange.End < DateTime.UtcNow)
            throw new BusinessException("A booking that has already ended cannot be modified.", 
                "HISTORICAL_BOOKING_MODIFICATION");
    }

    public static void ChangeTerminatedBooking(this IGuardClause guardClause, Booking booking)
    {
        guardClause.Null(booking, nameof(booking));
        if (BookingStatusExtensions.TerminalStates.Contains(booking.Status))
            throw new BusinessException($"A {nameof(booking.Status)} booking cannot be modified.", 
                "CANNOT_CHANGE_TERMINATED_BOOKING");
    }

    public static void InvalidChangeBookingStatus(this IGuardClause guardClause, BookingStatus currentStatus, BookingStatus newStatus)
    {
        guardClause.Null(currentStatus, nameof(currentStatus));
        guardClause.Null(newStatus, nameof(newStatus));
        if (BookingStatusExtensions.TerminalStates.Contains(currentStatus) && BookingStatusExtensions.ActiveStates.Contains(newStatus))
            throw new BusinessException($"The booking status cannot be changed from {nameof(currentStatus)} to {nameof(newStatus)}.", 
                "INVALID_BOOKING_STATUS_CHANGE");
    }
    public static void ContactCollisions(this IGuardClause guardClause, Booking booking, IReadOnlyList<Booking> existingBookings)
    {
        guardClause.Null(booking, nameof(booking));
        guardClause.Null(existingBookings, nameof(existingBookings));

        if (HasCollision(existingBookings, x => x.Contact.Id == booking.Contact.Id && x.TimeRange.Overlaps(booking.TimeRange)))
        {
            throw new BusinessException(
                $"The contact {booking.Contact.FullName} with license #{booking.Contact.LicenseNumber} is already booked for the specified time range.",
                "BOOKING_CONTACT_COLLISION");
        }
    }

    public static void CarHireCollisions(this IGuardClause guardClause, Booking booking, IReadOnlyList<Booking> existingBookings)
    {
        guardClause.Null(booking, nameof(booking));
        guardClause.Null(existingBookings, nameof(existingBookings));

        if (HasCollision(existingBookings, x => x.Car.Id == booking.Car.Id && x.TimeRange.Overlaps(booking.TimeRange)))
        {
            throw new BusinessException(
                $"The car with license plate {booking.Car.RegistrationNumber} is already booked for the specified time range.",
                "BOOKING_CAR_COLLISION");
        }
    }

    public static void Collisions(this IGuardClause guardClause, Booking booking, IReadOnlyList<Booking> collisionCandidates)
    {
        Collisions(guardClause, booking, booking.TimeRange, collisionCandidates);
    }

    public static void Collisions(this IGuardClause guardClause, Booking booking, DateTimeRange timeRange, IReadOnlyList<Booking> collisionCandidates)
    {
        if (!collisionCandidates.Any()) return;

        var errorMessage = new StringBuilder();

        CheckAndAppendError(booking, timeRange, collisionCandidates, HasContactCollision, errorMessage);
        CheckAndAppendError(booking, timeRange, collisionCandidates, HasCarCollision, errorMessage);

        if (errorMessage.Length > 0)
            throw new BusinessException(errorMessage.ToString(), "BOOKING_COLLISION");
    }

    private static void CheckAndAppendError(
        Booking booking, 
        DateTimeRange timeRange, 
        IReadOnlyList<Booking> collisionCandidates, 
        Func<Booking, DateTimeRange, IReadOnlyList<Booking>, string?> collisionChecker, 
        StringBuilder errorMessage)
    {
        var error = collisionChecker(booking, timeRange, collisionCandidates);
        if (!string.IsNullOrEmpty(error))
        {
            errorMessage.AppendLine(error);
        }
    }

    private static string? HasContactCollision(Booking booking, DateTimeRange timeRange, IReadOnlyList<Booking> collisionCandidates)
    {
        if (!HasCollision(collisionCandidates, x => x.Contact.Equals(booking.Contact) && x.TimeRange.Overlaps(timeRange) && !x.Equals(booking) && BookingStatusExtensions.ActiveStates.Contains(x.Status)))
            return null;

        return $"The contact {booking.Contact.FullName} with license #{booking.Contact.LicenseNumber} already has a booking between {timeRange.Start:HH:mm} to {timeRange.End:HH:mm}.";
    }

    private static string? HasCarCollision(Booking booking, DateTimeRange timeRange, IReadOnlyList<Booking>? collisionCandidates)
    {
        if (!HasCollision(collisionCandidates, x => x.Car.Equals(booking.Car) && x.TimeRange.Overlaps(timeRange) && !x.Equals(booking) && BookingStatusExtensions.ActiveStates.Contains(x.Status)))
            return null;

        return $"The car with license plate #{booking.Car.RegistrationNumber} is already reserved between {timeRange.Start:HH:mm} to {timeRange.End:HH:mm}.";
    }

    private static bool HasCollision(IReadOnlyList<Booking>? bookings, Func<Booking, bool> predicate)
    {
        return bookings != null && bookings.Any(predicate);
    }

}
