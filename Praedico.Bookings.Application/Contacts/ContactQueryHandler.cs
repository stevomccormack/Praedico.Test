using Praedico.Bookings.Domain.Contacts;

//Mediator - typically use Mediatr for CQRS handling

namespace Praedico.Bookings.Application.Contacts;

public class ContactQueryHandler
{
    private IContactQueryRepository ContactQueryRepository{ get; }

    public ContactQueryHandler(IContactQueryRepository carQueryRepository)
    {
        ContactQueryRepository = carQueryRepository;
    }
    
    public async Task<IReadOnlyList<Contact>> GetAllContacts(CancellationToken cancellationToken = default)
    {
        return await ContactQueryRepository.ListAsync(cancellationToken: cancellationToken);
    }

    public async Task<Contact?> GetContactByReference(string bookingReference, CancellationToken cancellationToken = default)
    {
        return await ContactQueryRepository.GetUniqueAsync(bookingReference, cancellationToken: cancellationToken);
    }
    
    public async Task<Contact?> GetUniqueAsync(string licenseNumber, CancellationToken cancellationToken = default)
    {
        return await ContactQueryRepository.GetUniqueAsync(licenseNumber, cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsUniqueAsync(string licenseNumber, CancellationToken cancellationToken = default)
    {
        return await ContactQueryRepository.ExistsUniqueAsync(licenseNumber, cancellationToken: cancellationToken);
    }
}