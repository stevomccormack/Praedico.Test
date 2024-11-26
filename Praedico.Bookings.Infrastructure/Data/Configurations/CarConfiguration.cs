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

        builder.OwnsOne(x => x.CarType,
            x =>
            {
                x.Property(p => p.Name).HasColumnName("CarType").HasMaxLength(20).IsRequired();
            }).Navigation(p => p.CarType).IsRequired();

        builder.OwnsOne(x => x.Status,
            x =>
            {
                x.Property(p => p.Name).HasColumnName("Status").HasMaxLength(20).IsRequired();
            }).Navigation(p => p.Status).IsRequired();
        
        Log.Debug($"{nameof(CarConfiguration)}:{nameof(IEntityTypeConfiguration<Car>)} Completed.");
    }
}