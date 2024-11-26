using Praedico.Exceptions;
using Praedico.Guards;

namespace Praedico.Bookings.Domain.Cars;

public class CarType : ValueObject
{
    public static readonly CarType Compact = new(nameof(Compact));
    public static readonly CarType Sedan = new(nameof(Sedan));
    public static readonly CarType SUV = new(nameof(SUV));
    public static readonly CarType Van = new(nameof(Van));
    
    public static IReadOnlyList<CarType> All => [Compact, Sedan, SUV, Van];
    public static IReadOnlyList<CarType> SmallCarTypes => [Compact, Sedan];
    public static IReadOnlyList<CarType> LargeCarTypes => [SUV, Van];     
    
    public string Name { get; }

    private CarType(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, "name");
        Name = name;
    }

    public static CarType FromName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, "name");
        if (All.Any(x => x.Name == name))
            return All.Single(x => x.Name == name);
        
        throw new BusinessException($"{nameof(CarType)} '" + name + "' is invalid.");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}

public static class CarTypeStatusExtensions
{
    public static CarType ToCarType(this string name) =>
        CarType.FromName(name);
}
