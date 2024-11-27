namespace Praedico.Bookings.Domain.Cars;

public enum CarType
{
    Compact,
    Sedan,
    SUV,
    Van
}

public static class CarTypeExtensions
{
    public static CarType[]? ToCarTypeArray(this string[]? carTypes)
    {
        return carTypes?.Select(carType => Enum.Parse<CarType>(carType, true)).ToArray() ?? null;
    }
}
