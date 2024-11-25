using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data.Configurations;
using Praedico.Bookings.Infrastructure.Data.Converters;

namespace Praedico.Bookings.Infrastructure.Data
{
    public class BookingsDbContext(DbContextOptions<BookingsDbContext> options) : DbContext(options)
    {
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Car> Cars { get; set; } = null!;
        public DbSet<Contact> Contacts { get; set; } = null!;
        public DbSet<Schedule> Schedules { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurations
            modelBuilder.ApplyConfiguration(new BookingConfiguration());
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            
            //Value Converters
            modelBuilder.ApplyValueConverters();

            // Seed Data
            modelBuilder.SeedAll();
        }
    }
}