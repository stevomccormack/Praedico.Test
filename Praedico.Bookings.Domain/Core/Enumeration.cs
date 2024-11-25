using Praedico.Guards;

namespace Praedico.Bookings.Domain;

/// <summary>
/// Enumerations - implements enumeration pattern
/// </summary>
public abstract class Enumeration : IEquatable<Enumeration>, IComparable<Enumeration>
{
    public string Name { get; }

    protected Enumeration(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    public override string ToString() => Name;

    public override bool Equals(object? obj) =>
        obj is Enumeration other && Equals(other);

    public bool Equals(Enumeration? other) =>
        other is not null && GetType() == other.GetType() && Name.Equals(other.Name, StringComparison.Ordinal);

    public override int GetHashCode() => Name.GetHashCode(StringComparison.Ordinal);

    public int CompareTo(Enumeration? other) =>
        other == null ? 1 : string.Compare(Name, other.Name, StringComparison.Ordinal);

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(System.Reflection.BindingFlags.Public |
                            System.Reflection.BindingFlags.Static |
                            System.Reflection.BindingFlags.DeclaredOnly)
            .Where(f => f.FieldType == typeof(T))
            .Select(f => (T)f.GetValue(null)!);

    public static T FromName<T>(string name) where T : Enumeration
    {
        Guard.Against.NullOrWhiteSpace(name, nameof(name));

        var matchingItem = GetAll<T>().FirstOrDefault(item => item.Name.Equals(name, StringComparison.Ordinal));
        if (matchingItem == null)
            throw new ArgumentException($"No {typeof(T).Name} with name '{name}' found.", nameof(name));

        return matchingItem;
    }
}