using Praedico.Guards;

namespace Praedico.Bookings.Domain;

public class DateTimeRange : IEquatable<DateTimeRange>, IComparable<DateTimeRange>
{
    public DateTime Start { get; }
    public DateTime End { get; }

    private DateTimeRange(DateTime start, DateTime end)
    {
        Guard.Against.StartAfterEnd(start, end, nameof(start), nameof(end));
        Start = start;
        End = end;
    }

    public DateTimeRange(DateTime start, TimeSpan duration) :
        this(start, start.Add(duration))
    {
    }

    public static DateTimeRange Create(DateTime start, DateTime end)
    {
        Guard.Against.Null(start, nameof(start));
        Guard.Against.Null(end, nameof(end));
        Guard.Against.StartAfterEnd(start, end, nameof(start), nameof(end));
        return new DateTimeRange(start, end);
    }
    
    public static DateTimeRange Create(DateTime start, TimeSpan duration)
    {
        Guard.Against.Null(start, nameof(start));
        Guard.Against.NegativeOrZero(duration, nameof(duration));
        
        return Create(start, start.Add(duration));
    }

    public bool Overlaps(DateTimeRange dateTimeRange)
    {
        return Start < dateTimeRange.End && End > dateTimeRange.Start;
    }

    public bool Contains(DateTime dateTime)
    {
        return dateTime >= Start && dateTime <= End;
    }

    public bool Collides(DateTimeRange dateTimeRange)
    {
        return StartsBeforeAndEndsAfter(dateTimeRange) ||
               StartsBeforeAndEndsDuring(dateTimeRange) ||
               StartsDuringAndEndsAfter(dateTimeRange) ||
               IsWithin(dateTimeRange);
    }

    private bool StartsBeforeAndEndsAfter(DateTimeRange other) =>
        Start <= other.Start && End >= other.End;

    private bool StartsBeforeAndEndsDuring(DateTimeRange other) =>
        Start <= other.Start && End > other.Start && End <= other.End;

    private bool StartsDuringAndEndsAfter(DateTimeRange other) =>
        Start >= other.Start && Start < other.End && End >= other.End;

    private bool IsWithin(DateTimeRange other) =>
        Start >= other.Start && End <= other.End;


    public override bool Equals(object? obj) => Equals(obj as DateTimeRange);

    public bool Equals(DateTimeRange? other)
    {
        if (other is null) return false;
        return Start == other.Start && End == other.End;
    }

    public override int GetHashCode() => HashCode.Combine(Start, End);

    public int CompareTo(DateTimeRange? other)
    {
        return other == null ? 1 : Start.CompareTo(other.Start);
    }

    public override string ToString() => $"{Start:O} - {End:O}";
}