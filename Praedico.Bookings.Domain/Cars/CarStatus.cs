namespace Praedico.Bookings.Domain.Cars;

public class CarStatus : Enumeration
{
    public static readonly CarStatus Available = new("Available");
    public static readonly CarStatus Unavailable = new("Unavailable");

    public static IReadOnlyList<CarStatus> All => [Available, Unavailable];
    public static IReadOnlyList<CarStatus> AvailableStates => [Available];
    public static IReadOnlyList<CarStatus> UnavailableStates => [Unavailable];

    private CarStatus(string name) : base(name) { }
}
