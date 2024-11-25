namespace Praedico.Bookings.Application.Contacts;

public record CreateContactRequest
{
    public string LicenseNumber { get; init; } = string.Empty;
    public string GivenName { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    
    public string? Email { get; init; } = string.Empty;
    public string? Phone { get; init; } = string.Empty;
}