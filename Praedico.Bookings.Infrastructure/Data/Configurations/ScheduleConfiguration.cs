using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Praedico.Bookings.Domain.Schedules;
using Serilog;

namespace Praedico.Bookings.Infrastructure.Data.Configurations;

internal class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
{
    public void Configure(EntityTypeBuilder<Schedule> builder)
    {
        Log.Debug($"{nameof(ScheduleConfiguration)}:{nameof(IEntityTypeConfiguration<Schedule>)} Started...");
        
        builder.HasKey(x => x.Id);
        builder.Ignore(x => x.DomainEvents);

        builder.Property(x => x.LocationCode).IsRequired();
        builder.Property(x => x.CreatedOn).HasColumnName("CreatedOnUtc").IsRequired().HasDefaultValueSql("GETUTCDATE()");

        builder.HasMany(x => x.Bookings)
            .WithOne()
            .HasForeignKey("ScheduleId")
            .OnDelete(DeleteBehavior.Cascade); // If a schedule is deleted, delete its bookings as well
        
        Log.Debug($"{nameof(ScheduleConfiguration)}:{nameof(IEntityTypeConfiguration<Schedule>)} Completed.");
    }
}