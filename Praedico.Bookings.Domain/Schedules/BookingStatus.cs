namespace Praedico.Bookings.Domain.Schedules;

public enum BookingStatus
{
    Placed,
    Confirmed,
    Completed,
    Cancelled,
    Abandoned
}

public static class BookingStatusExtensions
{
    public static BookingStatus[] ActiveStates =>
    [
        BookingStatus.Placed,
        BookingStatus.Confirmed
    ];
    
    public static BookingStatus[] TerminalStates =>
    [
        BookingStatus.Completed,
        BookingStatus.Cancelled,
        BookingStatus.Abandoned
    ];
}