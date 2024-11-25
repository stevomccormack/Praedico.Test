using Praedico.Bookings.Application.Contacts;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories
{
    public class ContactCommandRepository : CommandRepository<Contact>, IContactCommandRepository
    {
        public ContactCommandRepository(BookingsDbContext dbContext):
            base(dbContext)
        {
            
        }
    }
}