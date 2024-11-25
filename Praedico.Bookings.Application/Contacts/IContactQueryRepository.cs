using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Domain.Contacts;

namespace Praedico.Bookings.Application.Contacts;

public interface IContactQueryRepository : IQueryRepository<Contact>
{
    Task<Contact?> GetUniqueAsync(string licenseNumber, CancellationToken cancellationToken = default);

    Task<bool> ExistsUniqueAsync(string licenseNumber, CancellationToken cancellationToken = default);
}