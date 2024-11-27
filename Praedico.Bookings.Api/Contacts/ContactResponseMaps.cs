using Praedico.Bookings.Domain.Contacts;

namespace Praedico.Bookings.Api.Contacts;

public static class ContactResponseMaps
{
    public static IReadOnlyList<ContactResponse> ToListResponse(this IReadOnlyList<Contact> contacts)
    {
        return contacts.Select(x => x.ToResponse()).ToList().AsReadOnly();
    }

    public static ContactResponse ToResponse(this Contact contact)
    {
        return new ContactResponse
        {
            //ContactId // dont leak domain
            LicenseNumber = contact.LicenseNumber,
            GivenName = contact.GivenName,
            Surname = contact.Surname,
            Email = contact.Email,
            Phone = contact.Phone,
            IsActive = contact.IsActive
        };
    }
}