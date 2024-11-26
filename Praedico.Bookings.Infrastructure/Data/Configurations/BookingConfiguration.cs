using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data.Converters;
using Serilog;

namespace Praedico.Bookings.Infrastructure.Data.Configurations;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        Log.Debug($"{nameof(BookingConfiguration)}:{nameof(IEntityTypeConfiguration<Booking>)} Started...");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.BookingReference).IsUnique();
        builder.Ignore(x => x.DomainEvents);
        
        builder.Property(x => x.BookingReference).IsRequired().HasMaxLength(50);
        builder.Property(x => x.PickupDateTime).HasColumnName("PickupDateTimeUtc").IsRequired();
        builder.Property(x => x.ReturnDateTime).HasColumnName("ReturnDateTimeUtc").IsRequired();        
        builder.Property(x => x.Status).HasConversion(ValueConverters.EnumerationConverter<BookingStatus>()).IsRequired().HasMaxLength(50);        
        builder.Property(x => x.CreatedOn).HasColumnName("CreatedOnUtc").IsRequired().HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(x => x.Contact)
            .WithMany() // No navigation from Contact back to Booking.
            //.HasForeignKey("ContactId")
            .IsRequired(); // Ensures Contact is required.

        builder.HasOne(x => x.Car)
            .WithMany() // No navigation from Car back to Booking.
            //.HasForeignKey("CarId")
            .IsRequired(); // Ensures Car is required.
        
        Log.Debug($"{nameof(BookingConfiguration)}:{nameof(IEntityTypeConfiguration<Booking>)} Completed.");
    }
}