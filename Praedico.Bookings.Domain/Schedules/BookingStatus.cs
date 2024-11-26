using Praedico.Exceptions;
using Praedico.Guards;

namespace Praedico.Bookings.Domain.Schedules;

public class BookingStatus : ValueObject
{
    public static readonly BookingStatus Placed = new(nameof(Placed));
    public static readonly BookingStatus Confirmed = new(nameof(Confirmed));
    public static readonly BookingStatus Completed = new(nameof(Completed));
    public static readonly BookingStatus Cancelled = new(nameof(Cancelled));
    public static readonly BookingStatus Abandoned = new(nameof(Abandoned));

    public static IReadOnlyList<BookingStatus> All => [Placed, Confirmed, Completed, Cancelled, Abandoned];
    public static IReadOnlyList<BookingStatus> ActiveStates => [Placed, Confirmed];
    public static IReadOnlyList<BookingStatus> TerminalStates => [Completed, Cancelled, Abandoned];

    public string Name { get; }

    private BookingStatus(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, "name");
        Name = name;
    }

    public static BookingStatus FromName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, "name");
        if (All.Any(x => x.Name == name))
            return All.Single(x => x.Name == name);
        
        throw new BusinessException($"{nameof(BookingStatus)} '" + name + "' is invalid.");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}

public static class BookingStatusExtensions
{
    public static BookingStatus ToBookingStatus(this string name) =>
        BookingStatus.FromName(name);
}