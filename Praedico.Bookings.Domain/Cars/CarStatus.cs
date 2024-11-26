using Praedico.Exceptions;
using Praedico.Guards;

namespace Praedico.Bookings.Domain.Cars;

public class CarStatus : ValueObject
{
    public static readonly CarStatus Available = new(nameof(Available));
    public static readonly CarStatus Unavailable = new(nameof(Available)); 
    
    public static IReadOnlyList<CarStatus> All => [Available, Unavailable];
    public static IReadOnlyList<CarStatus> AvailableStates => [Available];
    public static IReadOnlyList<CarStatus> UnavailableStates => [Unavailable];       
    
    public string Name { get; }

    private CarStatus(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, "name");
        Name = name;
    }

    public static CarStatus FromName(string name)
    {
        Guard.Against.NullOrWhiteSpace(name, "name");
        if (All.Any(x => x.Name == name))
            return All.Single(x => x.Name == name);
        
        throw new BusinessException($"{nameof(CarStatus)} '" + name + "' is invalid.");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
    }
}

public static class CarStatusExtensions
{
    public static CarStatus ToCarStatus(this string name) =>
        CarStatus.FromName(name);
}
