namespace Praedico.Bookings.Domain;

public abstract class ValueObject : IComparable, IComparable<ValueObject>
{
    private int? _cachedHashCode;

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || GetType() != GetUnproxiedType(obj))
            return false;

        var valueObject = (ValueObject)obj;

        return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        if (_cachedHashCode.HasValue)
            return _cachedHashCode.Value;

        _cachedHashCode = GetEqualityComponents()
            .Aggregate(1, (current, component) =>
            {
                unchecked
                {
                    return current * 23 + (component?.GetHashCode() ?? 0);
                }
            });

        return _cachedHashCode.Value;
    }

    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;

        var thisType = GetType();
        var otherType = GetUnproxiedType(obj);

        if (thisType != otherType)
            return string.Compare(thisType.Name, otherType.Name, StringComparison.Ordinal);

        var other = (ValueObject)obj;

        return CompareComponents(GetEqualityComponents(), other.GetEqualityComponents());
    }

    private static int CompareComponents(IEnumerable<object> components, IEnumerable<object> otherComponents)
    {
        using var thisEnumerator = components.GetEnumerator();
        using var otherEnumerator = otherComponents.GetEnumerator();

        while (thisEnumerator.MoveNext() && otherEnumerator.MoveNext())
        {
            var comparison = CompareSingleComponent(thisEnumerator.Current, otherEnumerator.Current);
            if (comparison != 0)
                return comparison;
        }

        return 0;
    }

    private static int CompareSingleComponent(object? a, object? b)
    {
        if (a is null && b is null) return 0;
        if (a is null) return -1;
        if (b is null) return 1;

        if (a is IComparable comparableA && b is IComparable comparableB)
            return comparableA.CompareTo(comparableB);

        return a.Equals(b) ? 0 : -1;
    }

    public int CompareTo(ValueObject? other) => CompareTo((object?)other);

    public static bool operator ==(ValueObject? a, ValueObject? b)
    {
        if (ReferenceEquals(a, b)) return true;
        if (a is null || b is null) return false;

        return a.Equals(b);
    }

    public static bool operator !=(ValueObject? a, ValueObject? b) => !(a == b);

    private static Type GetUnproxiedType(object obj)
    {
        const string proxyPrefix = "Castle.Proxies."; // EF Core proxies
        const string proxyPostfix = "Proxy"; // NHibernate proxies

        var type = obj.GetType();
        var typeName = type.FullName ?? type.Name;

        return (typeName.Contains(proxyPrefix) || typeName.EndsWith(proxyPostfix))
            ? type.BaseType ?? type
            : type;
    }
}
