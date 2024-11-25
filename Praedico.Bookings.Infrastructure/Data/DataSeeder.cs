using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;
using Serilog;

namespace Praedico.Bookings.Infrastructure.Data
{
    public static class DataSeeder
    {
        
        public static void SeedAll(this ModelBuilder modelBuilder)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedAll)} Started...");
            
            SeedCars(modelBuilder);
            SeedContacts(modelBuilder);
            SeedBookings(modelBuilder);
            SeedSchedule(modelBuilder);
            
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedAll)} Completed.");
        }

        private static void SeedCars(this ModelBuilder modelBuilder)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedCars)} Started...");
            
            SeedData.SeedCars();
            modelBuilder.Entity<Car>().HasData(SeedData.Cars.Select(x => new
            {
                x.Id,
                x.CarType,
                x.RegistrationNumber,
                x.Status,
                x.IsActive
            }));
            
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedCars)} Completed.");
        }

        private static void SeedContacts(this ModelBuilder modelBuilder)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedContacts)} Started...");
            
            SeedData.SeedContacts();
            modelBuilder.Entity<Contact>().HasData(SeedData.Contacts.Select(x => new
            {
                x.Id,
                x.LicenseNumber,
                x.GivenName,
                x.Surname,
                x.Email,
                x.Phone,
                x.IsActive
            }));
            
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedContacts)} Completed.");
        }

        private static void SeedBookings(this ModelBuilder modelBuilder)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedSchedule)} Started...");
            
            SeedData.SeedBookings();
            modelBuilder.Entity<Booking>().HasData(SeedData.Bookings.Select(x => new
            {
                x.Id,
                x.BookingReference,
                ContactId = x.Contact.Id,       // Foreign key for Contact
                CarId = x.Car.Id,               // Foreign key for Car
                x.PickupDateTime, 
                x.ReturnDateTime,
                x.TimeRange,
                x.Status,
                x.StatusChangedOn,
                x.CreatedOn,
                x.LastModifiedOn
            }));
                
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedSchedule)} Completed.");
        }

        private static void SeedSchedule(this ModelBuilder modelBuilder)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedSchedule)} Started...");
            
            SeedData.SeedSchedules();
            modelBuilder.Entity<Schedule>().HasData(SeedData.Schedules.Select(x => new
            {
                x.Id,
                x.LocationCode,
                x.CreatedOn
            }));
            
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedSchedule)} Completed.");
        }
    }

    internal static class SeedData
    {
        public static List<Car> Cars { get; private set; } = [];
        public static List<Contact> Contacts { get; private set; } = [];
        public static List<Booking> Bookings { get; private set; } = [];
        public static List<Schedule> Schedules { get; private set; } = [];

        public static void SeedAll()
        {
            SeedCars();
            SeedContacts();
            SeedBookings();
            SeedSchedules();
        }

        public static void SeedCars()
        {
            Cars =
            [
                Car.Create(CarType.Compact, "SPIDERMAN"),
                Car.Create(CarType.Compact, "IRONMAN"),
                Car.Create(CarType.Van, "HULK"),
                Car.Create(CarType.SUV, "SUPERMAN"),
                Car.Create(CarType.Compact, "BATMOBILE"),
                Car.Create(CarType.Compact, "LEXCORP")
            ];
        }

        public static void SeedContacts()
        {
            Contacts =
            [
                Contact.Create("MARV01", "Peter", "Parker"),
                Contact.Create("MARV02", "Tony", "Stark"),
                Contact.Create("MARV03", "Bruce", "Banner"),
                Contact.Create("DC0001", "Clark", "Kent"),
                Contact.Create("DC0002", "Bruce", "Wayne"),
                Contact.Create("DC0003", "Lex", "Luthor")
            ];
        }

        public static void SeedBookings()
        {
            Bookings =
            [
                Booking.Create(
                    Contacts.First(x => x.LicenseNumber == "MARV01"),
                    Cars.First(x => x.RegistrationNumber == "SPIDERMAN"),
                    DateTime.UtcNow.AddHours(1),
                    DateTime.UtcNow.AddDays(1)
                ),

                Booking.Create(
                    Contacts.First(x => x.LicenseNumber == "MARV02"),
                    Cars.First(x => x.RegistrationNumber == "IRONMAN"),
                    DateTime.UtcNow.AddHours(2),
                    DateTime.UtcNow.AddDays(2)
                ),

                Booking.Create(
                    Contacts.First(x => x.LicenseNumber == "MARV03"),
                    Cars.First(x => x.RegistrationNumber == "HULK"),
                    DateTime.UtcNow.AddHours(3),
                    DateTime.UtcNow.AddDays(3)
                ),

                Booking.Create(
                    Contacts.First(x => x.LicenseNumber == "DC0001"),
                    Cars.First(x => x.RegistrationNumber == "SUPERMAN"),
                    DateTime.UtcNow.AddHours(4),
                    DateTime.UtcNow.AddDays(4)
                ),

                Booking.Create(
                    Contacts.First(x => x.LicenseNumber == "DC0002"),
                    Cars.First(x => x.RegistrationNumber == "BATMOBILE"),
                    DateTime.UtcNow.AddHours(5),
                    DateTime.UtcNow.AddDays(5)
                ),

                Booking.Create(
                    Contacts.First(x => x.LicenseNumber == "DC0003"),
                    Cars.First(x => x.RegistrationNumber == "LEXCORP"),
                    DateTime.UtcNow.AddHours(6),
                    DateTime.UtcNow.AddDays(6)
                )

            ];
        }

        public static void SeedSchedules()
        {
            Schedules =
            [
                Schedule.Create("ASGARD"),
                Schedule.Create("METROPOLIS")
            ];
        }
    }
}