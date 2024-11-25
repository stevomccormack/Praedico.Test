using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Infrastructure.Data.Configurations;

internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        // Primary Key
        builder.HasKey(x => x.Id);

        // Properties
        builder.Property(x => x.LocationCode)
            .IsRequired();
        
        builder.Property(x => x.CreatedOn)
            .IsRequired();

        // Relationships
        builder.HasMany(x => x.Bookings)
            .WithOne()
            .HasForeignKey("ScheduleId") // Foreign key in the Bookings table
            .OnDelete(DeleteBehavior.Cascade); // If a schedule is deleted, delete its bookings as well
    }
}