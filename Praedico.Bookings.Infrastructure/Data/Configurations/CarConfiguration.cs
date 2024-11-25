using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain;
using Praedico.Bookings.Domain.Cars;

namespace Praedico.Bookings.Infrastructure.Data.Configurations;

internal class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(x => x.CarType)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        // Indexes
        builder.HasIndex(x => x.RegistrationNumber, "IX_Car_RegistrationNumber")
            .IsUnique();

        // Enumerations
        builder.Property(x => x.CarType)
            .HasConversion(
                convertToProviderExpression: v => v.Name,
                convertFromProviderExpression: v => Enumeration.FromName<CarType>(v)
            )
            .HasMaxLength(50);

        builder.Property(x => x.Status)
            .HasConversion(
                convertToProviderExpression: v => v.Name,
                convertFromProviderExpression: v => Enumeration.FromName<CarHireStatus>(v) 
            )
            .HasMaxLength(50);
    }
}