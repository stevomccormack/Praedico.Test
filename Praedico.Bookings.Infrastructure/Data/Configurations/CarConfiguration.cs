using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain.Cars;
using Serilog;

namespace Praedico.Bookings.Infrastructure.Data.Configurations;

internal class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        Log.Debug($"{nameof(CarConfiguration)}:{nameof(IEntityTypeConfiguration<Car>)} Started...");
        
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.RegistrationNumber).IsUnique();
        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.RegistrationNumber).IsRequired().HasMaxLength(20);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(false); 
        builder.Property(x => x.CarType).HasConversion<string>().HasMaxLength(20).IsRequired().HasDefaultValue(CarType.Compact);
        builder.Property(x => x.Status).HasConversion<string>().HasMaxLength(20).IsRequired().HasDefaultValue(CarStatus.Available);   
        
        Log.Debug($"{nameof(CarConfiguration)}:{nameof(IEntityTypeConfiguration<Car>)} Completed.");
    }
}