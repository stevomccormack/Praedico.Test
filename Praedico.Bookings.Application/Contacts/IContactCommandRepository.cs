using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Domain.Contacts;

namespace Praedico.Bookings.Application.Contacts;

public interface IContactCommandRepository : ICommandRepository<Contact>
{
        
}