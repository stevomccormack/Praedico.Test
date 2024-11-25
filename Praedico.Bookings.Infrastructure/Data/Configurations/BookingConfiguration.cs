using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Infrastructure.Data.Configurations;

internal class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.BookingReference)
            .IsRequired()
            .HasMaxLength(50);

        builder.OwnsOne(x => x.TimeRange, timeRange =>
        {
            timeRange.Property(tr => tr.Start)
                //.HasColumnName("PickupDateTime")
                .IsRequired();

            timeRange.Property(tr => tr.End)
                //.HasColumnName("ReturnDateTime")
                .IsRequired();
        }).Navigation(p => p.TimeRange).IsRequired();

        // Ignore computed
        builder.Ignore(x => x.PickupDateTime);
        builder.Ignore(x => x.ReturnDateTime);

        builder.Property(x => x.Status)
            .IsRequired()
            .HasConversion(
                convertToProviderExpression: v => v.Name,
                convertFromProviderExpression: v => Enumeration.FromName<BookingStatus>(v)
            )
            .HasMaxLength(50);

        builder.Property(x => x.CreatedOn)
            .IsRequired();

        builder.Property(x => x.LastModifiedOn);

        builder.Property(x => x.StatusChangedOn);

        // Indexes
        builder.HasIndex(x => x.BookingReference, "IX_Booking_BookingReference")
            .IsUnique();

        // Relationships
        builder.HasOne(x => x.Contact)
            .WithMany()
            .HasForeignKey("ContactId")
            .IsRequired();

        builder.HasOne(x => x.Car)
            .WithMany()
            .HasForeignKey("CarId")
            .IsRequired();
    }
}