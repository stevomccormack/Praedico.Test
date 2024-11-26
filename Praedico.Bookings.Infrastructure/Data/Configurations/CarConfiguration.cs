using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Infrastructure.Data.Converters;
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
        builder.Property(x => x.CarType).HasConversion(ValueConverters.EnumerationConverter<CarType>()).IsRequired().HasMaxLength(50);        
        builder.Property(x => x.Status).HasConversion(ValueConverters.EnumerationConverter<CarStatus>()).IsRequired().HasMaxLength(50);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(false);        
        
        Log.Debug($"{nameof(CarConfiguration)}:{nameof(IEntityTypeConfiguration<Car>)} Completed.");
    }
}