using Praedico.Bookings.Application.Contacts;

//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Contacts;

public class ContactApiQueryHandler(ContactQueryHandler contactQueryHandler)
{
    private ContactQueryHandler ContactQueryHandler{ get; } = contactQueryHandler;

    public async Task<IResult> GetAllContacts(CancellationToken cancellationToken = default)
    {
        var contacts = await ContactQueryHandler.GetAllContacts(cancellationToken: cancellationToken);
        var result = contacts.ToListResponse();
        return Results.Ok(result);
    }

    public async Task<IResult> GetContactByRegistrationNumber(string licenseNumber, CancellationToken cancellationToken = default)
    {
        var contact = await ContactQueryHandler.GetUniqueAsync(licenseNumber, cancellationToken: cancellationToken);
        return contact == null
            ? Results.NotFound($"Contact with license number #{licenseNumber} not found.")
            : Results.Ok(contact.ToResponse());
    }
}