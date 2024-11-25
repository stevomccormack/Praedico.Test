using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain.Contacts;

namespace Praedico.Bookings.Infrastructure.Data.Configurations;

internal class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.LicenseNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.GivenName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Email)
            .HasMaxLength(100);

        builder.Property(x => x.Phone)
            .HasMaxLength(15);

        // Indexes
        builder.HasIndex(x => x.LicenseNumber, "IX_Contact_LicenseNumber")
            .IsUnique();

        // Ignore computed
        builder.Ignore(x => x.FullName);
    }
}