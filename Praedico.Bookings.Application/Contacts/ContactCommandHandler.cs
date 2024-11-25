using Praedico.Bookings.Domain.Contacts;

namespace Praedico.Bookings.Application.Contacts;

public class ContactCommandHandler
{
    private IContactCommandRepository ContactCommandRepository{ get; }

    public ContactCommandHandler(IContactCommandRepository carCommandRepository)
    {
        ContactCommandRepository = carCommandRepository;
    }

    public async Task<Contact> CreateContact(CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        var contact = Contact.Create(request.LicenseNumber, request.GivenName, request.Surname);
        contact.SetEmail(request.Email);
        contact.SetPhone(request.Phone);
        
        return await ContactCommandRepository.CreateAsync(contact, cancellationToken: cancellationToken);
    }

    public void UpdateContact(string licenseNumber, CreateContactRequest request, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
    }

    public void DeleteContact(string licenseNumber, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
    }
}