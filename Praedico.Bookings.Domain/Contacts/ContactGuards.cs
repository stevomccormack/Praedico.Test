using System.Text.RegularExpressions;
using Praedico.Exceptions;
using Praedico.Guards;

namespace Praedico.Bookings.Domain.Contacts;

public static class ContactGuards
{
    public static void InvalidLicenseNumber(this IGuardClause guardClause, string licenseNumber)
    {
        if (!Regex.IsMatch(licenseNumber, @"^[a-zA-Z0-9]+$")) // alphanumeric
            throw new BusinessException($"The license number must be alphanumeric: {licenseNumber}.", 
                "INVALID_CONTACT_LICENSE");
    }
    
    public static void InactiveContact(this IGuardClause guardClause, Contact contact)
    {
        if (!contact.IsActive)
            throw new BusinessException($"Inactive contact: {contact.FullName}.", 
                "INACTIVE_CONTACT");
    }
}
