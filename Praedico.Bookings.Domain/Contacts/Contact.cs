using Praedico.Guards;

namespace Praedico.Bookings.Domain.Contacts;

public class Contact : Entity
{
    public string LicenseNumber { get; private set; } //unique
    public string GivenName { get; private set; }
    public string Surname { get; private set; }
    public string FullName => $"{GivenName} {Surname}";
    public string? Email { get; private set; } // receipts
    public string? Phone { get; private set; } // direct contact
    public bool IsActive { get; private set; } = true;

    private Contact(Guid id, string licenseNumber, string givenName, string surname) : base(id)
    {
        LicenseNumber = licenseNumber;
        GivenName = givenName;
        Surname = surname;
    }
    
    public static Contact Create(string licenseNumber, string givenName, string surname)
    {
        Guard.Against.NullOrWhiteSpace(licenseNumber, nameof(licenseNumber));
        Guard.Against.NullOrWhiteSpace(givenName, nameof(givenName));
        Guard.Against.NullOrWhiteSpace(surname, nameof(surname));
        Guard.Against.InvalidLicenseNumber(licenseNumber);

        return new Contact(Guid.NewGuid(), licenseNumber, givenName, surname);
    }
    
    public void ChangeLicenseNumber(string licenseNumber)
    {
        Guard.Against.NullOrWhiteSpace(licenseNumber, nameof(licenseNumber));
        Guard.Against.InvalidLicenseNumber(licenseNumber);

        LicenseNumber = licenseNumber;
    }
    
    public void ChangeFullName(string givenName, string surname)
    {
        Guard.Against.NullOrWhiteSpace(givenName, nameof(givenName));
        Guard.Against.NullOrWhiteSpace(surname, nameof(surname));

        GivenName = givenName;
        Surname = surname;
    }
    
    public void SetEmail(string email)
    {
        Guard.Against.NullOrWhiteSpace(email, nameof(email));
        Guard.Against.InvalidEmail(email);
        
        Email = email;
    }
    
    public void SetPhone(string phone)
    {
        Guard.Against.NullOrWhiteSpace(phone, nameof(phone));
        Guard.Against.InvalidPhone(phone);
        
        Phone = phone;
    }

    public void Activate()
    {
        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (IsActive)
            IsActive = false;
    }
}
