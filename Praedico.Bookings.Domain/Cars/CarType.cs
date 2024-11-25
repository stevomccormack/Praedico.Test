namespace Praedico.Bookings.Domain.Cars;

public class CarType : Enumeration
{
    public static readonly CarType Compact = new("Compact");
    public static readonly CarType Sedan = new("Sedan");
    public static readonly CarType SUV = new("SUV");
    public static readonly CarType Van = new("Van");
    
    public static IReadOnlyList<CarType> All => new[] { Compact, Sedan, SUV, Van };
    public static IReadOnlyList<CarType> SmallCarTypes => new[] { Compact, Sedan };
    public static IReadOnlyList<CarType> LargeCarTypes => new[] { SUV, Van };

    private CarType(string name) : base(name) { }
}