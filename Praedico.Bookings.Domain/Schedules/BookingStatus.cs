namespace Praedico.Bookings.Domain.Schedules;

public class BookingStatus : Enumeration
{
    public static readonly BookingStatus Placed = new("Placed");
    public static readonly BookingStatus Confirmed = new("Confirmed");
    public static readonly BookingStatus Completed = new("Completed");
    public static readonly BookingStatus Cancelled = new("Cancelled");
    public static readonly BookingStatus Abandoned = new("Abandoned");

    public static IReadOnlyList<BookingStatus> All => [Placed, Confirmed, Completed, Cancelled, Abandoned];
    public static IReadOnlyList<BookingStatus> ActiveStates => [Placed, Confirmed];
    public static IReadOnlyList<BookingStatus> TerminalStates => [Completed, Cancelled, Abandoned];

    private BookingStatus(string name) : base(name) { }
}
