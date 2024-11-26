using Praedico.Bookings.Application.Contacts;
using Praedico.Bookings.Domain.Contacts;

namespace Praedico.Bookings.Api.Contacts;

public static class ContactRequestMap
{
    public static Contact ToDomain(this CreateContactRequest request)
    {
        var contact = Contact.Create(request.LicenseNumber, request.GivenName, request.Surname);
        contact.SetEmail(request.Email); 
        contact.SetPhone(request.Phone);
        return contact;
    }
}