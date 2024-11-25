using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain.Contacts;
using Serilog;

namespace Praedico.Bookings.Infrastructure.Data.Configurations;

internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        Log.Debug($"{nameof(ContactConfiguration)}:{nameof(IEntityTypeConfiguration<Contact>)} Started...");
        
        builder.HasKey(x => x.Id);
        builder.Ignore(x => x.FullName);
        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.LicenseNumber).IsRequired().HasMaxLength(20);
        builder.Property(x => x.GivenName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Surname).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Email).HasMaxLength(100);
        builder.Property(x => x.Phone).HasMaxLength(20);

        builder.HasIndex(x => x.LicenseNumber).IsUnique();
        
        Log.Debug($"{nameof(ContactConfiguration)}:{nameof(IEntityTypeConfiguration<Contact>)} Completed.");
    }
}