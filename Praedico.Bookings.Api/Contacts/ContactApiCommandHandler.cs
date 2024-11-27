using Praedico.Bookings.Application.Contacts;

//Mediator - typically use for CQRS handling

namespace Praedico.Bookings.Api.Contacts;

public class ContactApiCommandHandler(ContactCommandHandler contactCommandHandler)
{
    private ContactCommandHandler ContactCommandHandler{ get; } = contactCommandHandler;

    public async Task<IResult> CreateContact(ContactRequest request, CancellationToken cancellationToken = default)
    {
        var contact = await ContactCommandHandler.CreateContact(request, cancellationToken: cancellationToken);
        var result = contact.ToResponse();
        return Results.Created($"/api/cars/{contact.LicenseNumber}", result);
    }

    public async Task<IResult> UpdateContact(string licenseNumber, ContactRequest request, CancellationToken cancellationToken = default)
    {
        await ContactCommandHandler.UpdateContact(licenseNumber, request, cancellationToken: cancellationToken);
        return Results.NoContent();
    }

    public async Task<IResult> DeleteContact(string licenseNumber, CancellationToken cancellationToken = default)
    {
        await ContactCommandHandler.DeleteContact(licenseNumber, cancellationToken: cancellationToken);
        return Results.NoContent();
    }
}