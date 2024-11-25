using Praedico.Guards;

namespace Praedico.Bookings.Domain;

public class DateTimeRange //ValueObject
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    private DateTimeRange(DateTime start, DateTime end)
    {
        Guard.Against.StartAfterEnd(start, end, nameof(start), nameof(end));

        Start = start;
        End = end;
    }

    public static DateTimeRange Create(DateTime start, DateTime end)
    {
        Guard.Against.Null(start, nameof(start));
        Guard.Against.Null(end, nameof(end));
        Guard.Against.StartAfterEnd(start, end, nameof(start), nameof(end));

        return new DateTimeRange(start, end);
    }

    public bool Overlaps(DateTimeRange dateTime)
    {
        Guard.Against.Null(dateTime, nameof(dateTime));
        
        return Start < dateTime.End && End > dateTime.Start;
    }

    public bool Contains(DateTime dateTime)
    {
        Guard.Against.Null(dateTime, nameof(dateTime));
        
        return dateTime >= Start && dateTime <= End;
    }

    public void SetRange(DateTime start, DateTime end)
    {
        Guard.Against.Null(start, nameof(start));
        Guard.Against.Null(end, nameof(end));
        Guard.Against.StartAfterEnd(start, end, nameof(start), nameof(end));

        Start = start;
        End = end;
    }
}
