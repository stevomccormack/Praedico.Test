namespace Praedico.Bookings.Api.Contacts;

public record ContactResponse
{
    public string LicenseNumber { get; init; } = string.Empty; // Unique
    public string GivenName { get; init; } = string.Empty;
    public string Surname { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; }
}