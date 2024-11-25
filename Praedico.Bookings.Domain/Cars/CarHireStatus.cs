namespace Praedico.Bookings.Domain.Cars;

public class CarHireStatus : Enumeration
{
    public static readonly CarHireStatus Available = new("Available");
    public static readonly CarHireStatus Hired = new("Hired");
    public static readonly CarHireStatus Unavailable = new("Unavailable");

    public static IReadOnlyList<CarHireStatus> All => new[] { Available, Hired, Unavailable };
    public static IReadOnlyList<CarHireStatus> AvailableStates => new[] { Available };
    public static IReadOnlyList<CarHireStatus> UnavailableStates => new[] { Hired, Unavailable };

    private CarHireStatus(string name) : base(name) { }
}
