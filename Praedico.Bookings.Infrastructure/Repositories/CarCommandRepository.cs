using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories
{
    public class CarCommandRepository : CommandRepository<Car>, ICarCommandRepository
    {
        public CarCommandRepository(BookingsDbContext dbContext):
            base(dbContext)
        {
            
        }
    }
}