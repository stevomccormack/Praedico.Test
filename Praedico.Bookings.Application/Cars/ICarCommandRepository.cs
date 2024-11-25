using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Application.Cars;

public interface ICarCommandRepository : ICommandRepository<Car>
{
        
}