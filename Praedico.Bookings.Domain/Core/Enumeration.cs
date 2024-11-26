using System.Reflection;

public abstract class Enumeration : IEquatable<Enumeration>, IComparable<Enumeration>
{
    public string Name { get; }

    protected Enumeration(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));
        Name = name;
    }

    public override string ToString() => Name;

    public override bool Equals(object? obj) => obj is Enumeration other && Equals(other);

    public bool Equals(Enumeration? other) =>
        other is not null && GetType() == other.GetType() && Name.Equals(other.Name, StringComparison.Ordinal);

    public override int GetHashCode() => Name.GetHashCode(StringComparison.Ordinal);

    public int CompareTo(Enumeration? other) =>
        other == null ? 1 : string.Compare(Name, other.Name, StringComparison.Ordinal);

    public static bool operator ==(Enumeration? left, Enumeration? right) => Equals(left, right);

    public static bool operator !=(Enumeration? left, Enumeration? right) => !Equals(left, right);

    public static T FromName<T>(string name) where T : Enumeration
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or empty.", nameof(name));

        return GetAll<T>().FirstOrDefault(e => e.Name == name)
               ?? throw new InvalidOperationException($"No {typeof(T).Name} with name '{name}' found.");
    }

    public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
        typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => (T)f.GetValue(null)!);
}