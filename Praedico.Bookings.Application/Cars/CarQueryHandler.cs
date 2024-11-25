using Praedico.Bookings.Domain.Cars;

//Mediator - typically use Mediatr for CQRS handling

namespace Praedico.Bookings.Application.Cars;

public class CarQueryHandler
{
    private ICarQueryRepository CarQueryRepository{ get; }

    public CarQueryHandler(ICarQueryRepository carQueryRepository)
    {
        CarQueryRepository = carQueryRepository;
    }
    
    public async Task<IReadOnlyList<Car>> GetAllCars(CancellationToken cancellationToken = default)
    {
        return await CarQueryRepository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Car?> GetCarByReference(string bookingReference, CancellationToken cancellationToken = default)
    {
        return await CarQueryRepository.GetUniqueAsync(bookingReference, cancellationToken: cancellationToken);
    }
    
    public async Task<Car?> GetUniqueAsync(string licenseNumber, CancellationToken cancellationToken = default)
    {
        return await CarQueryRepository.GetUniqueAsync(licenseNumber, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsUniqueAsync(string licenseNumber, CancellationToken cancellationToken = default)
    {
        return await CarQueryRepository.ExistsUniqueAsync(licenseNumber, cancellationToken: cancellationToken);
    }
}