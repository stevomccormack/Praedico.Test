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
        public static void SeedAll(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedAll)} Started...");
            
            SeedCars(dbContext);
            SeedContacts(dbContext);
            SeedSchedules(dbContext);
            SeedBookings(dbContext);
            
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedAll)} Completed.");
        }

        private static void SeedCars(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedCars)} Started...");

            if (!dbContext.Cars.Any())
            {
                SeedData.SeedCars();
                dbContext.Cars.AddRange(SeedData.Cars);
                dbContext.SaveChanges();
            }

            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedCars)} Completed.");
        }

        private static void SeedContacts(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedContacts)} Started...");

            if (!dbContext.Contacts.Any())
            {
                SeedData.SeedContacts();
                dbContext.Contacts.AddRange(SeedData.Contacts);
                dbContext.SaveChanges();
            }

            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedContacts)} Completed.");
        }

        private static void SeedSchedules(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedSchedules)} Started...");

            if (!dbContext.Schedules.Any())
            {
                SeedData.SeedSchedules();
                dbContext.Schedules.AddRange(SeedData.Schedules);
                dbContext.SaveChanges();
            }

            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedSchedules)} Completed.");
        }

        private static void SeedBookings(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedBookings)} Started...");

            if (!dbContext.Bookings.Any())
            {
                var contacts = dbContext.Contacts.ToList();
                var cars = dbContext.Cars.ToList();
                SeedData.SeedBookings(contacts, cars);
                
                dbContext.Bookings.AddRange(SeedData.Bookings);
                dbContext.SaveChanges();
            }

            Log.Debug($"{nameof(DataSeeder)}:{nameof(SeedBookings)} Completed.");
        }
    }

    internal static class SeedData
    {
        public static List<Car> Cars { get; private set; } = [];
        public static List<Contact> Contacts { get; private set; } = [];
        public static List<Booking> Bookings { get; private set; } = [];
        public static List<Schedule> Schedules { get; private set; } = [];

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

        public static void SeedBookings(List<Contact> contacts, List<Car> cars)
        {
            Bookings =
            [
                Booking.Create(
                    contacts.First(x => x.LicenseNumber == "MARV01"),
                    cars.First(x => x.RegistrationNumber == "SPIDERMAN"),
                    DateTime.UtcNow.AddHours(1),
                    DateTime.UtcNow.AddDays(1)
                ),
                Booking.Create(
                    contacts.First(x => x.LicenseNumber == "MARV02"),
                    cars.First(x => x.RegistrationNumber == "IRONMAN"),
                    DateTime.UtcNow.AddHours(2),
                    DateTime.UtcNow.AddDays(2)
                ),
                Booking.Create(
                    contacts.First(x => x.LicenseNumber == "MARV03"),
                    cars.First(x => x.RegistrationNumber == "HULK"),
                    DateTime.UtcNow.AddHours(3),
                    DateTime.UtcNow.AddDays(3)
                ),
                Booking.Create(
                    contacts.First(x => x.LicenseNumber == "DC0001"),
                    cars.First(x => x.RegistrationNumber == "SUPERMAN"),
                    DateTime.UtcNow.AddHours(4),
                    DateTime.UtcNow.AddDays(4)
                ),
                Booking.Create(
                    contacts.First(x => x.LicenseNumber == "DC0002"),
                    cars.First(x => x.RegistrationNumber == "BATMOBILE"),
                    DateTime.UtcNow.AddHours(5),
                    DateTime.UtcNow.AddDays(5)
                ),
                Booking.Create(
                    contacts.First(x => x.LicenseNumber == "DC0003"),
                    cars.First(x => x.RegistrationNumber == "LEXCORP"),
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