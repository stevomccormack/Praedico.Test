using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;
using Serilog;

namespace Praedico.Bookings.Infrastructure.Data
{
    public static class DataSeedContext
    {
        public static void SeedAll(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedAll)} Started...");
            
            SeedCars(dbContext);
            SeedContacts(dbContext);
            SeedSchedules(dbContext);
            SeedBookings(dbContext);
            
            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedAll)} Completed.");
        }

        private static void SeedCars(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedCars)} Started...");

            if (!dbContext.Cars.Any())
                DataSeeder.SeedCars(dbContext);

            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedCars)} Completed.");
        }

        private static void SeedContacts(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedContacts)} Started...");

            if (!dbContext.Contacts.Any())
                DataSeeder.SeedContacts(dbContext);

            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedContacts)} Completed.");
        }

        private static void SeedSchedules(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedSchedules)} Started...");

            if (!dbContext.Schedules.Any())
                DataSeeder.SeedSchedules(dbContext);

            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedSchedules)} Completed.");
        }

        private static void SeedBookings(this BookingsDbContext dbContext)
        {
            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedBookings)} Started...");

            if (!dbContext.Bookings.Any())
                DataSeeder.SeedBookings(dbContext);

            Log.Debug($"{nameof(DataSeedContext)}:{nameof(SeedBookings)} Completed.");
        }
    }
    
    internal static class DataSeeder
    {
        public static void SeedCars(BookingsDbContext dbContext)
        {
            var cars = new List<Car>
            {
                Car.Create("SPIDERMAN", CarType.Compact),
                Car.Create("IRONMAN", CarType.Compact),
                Car.Create("HULK", CarType.Van),
                Car.Create("SUPERMAN", CarType.SUV),
                Car.Create("BATMOBILE", CarType.Compact),
                Car.Create("LEXCORP", CarType.Compact)
            };

            dbContext.Cars.AddRange(cars);
            dbContext.SaveChanges();
        }

        public static void SeedContacts(BookingsDbContext dbContext)
        {
            var contacts = new List<Contact>
            {
                Contact.Create("MARV01", "Peter", "Parker"),
                Contact.Create("MARV02", "Tony", "Stark"),
                Contact.Create("MARV03", "Bruce", "Banner"),
                Contact.Create("DC0001", "Clark", "Kent"),
                Contact.Create("DC0002", "Bruce", "Wayne"),
                Contact.Create("DC0003", "Lex", "Luthor")
            };

            foreach(var contact in contacts)
                dbContext.Contacts.Add(contact);
            //dbContext.Contacts.AddRange(contacts);
            dbContext.SaveChanges();
        }

        public static void SeedSchedules(BookingsDbContext dbContext)
        {
            var schedules = new List<Schedule>
            {
                Schedule.Create("ASGARD"),
                Schedule.Create("METROPOLIS")
            };

            dbContext.Schedules.AddRange(schedules);
            dbContext.SaveChanges();
        }

        public static void SeedBookings(BookingsDbContext dbContext)
        {
            var bookings = new List<Booking>
            {
                Booking.Create(
                    dbContext.Contacts.First(x => x.LicenseNumber == "MARV01"),
                    dbContext.Cars.First(x => x.RegistrationNumber == "SPIDERMAN"),
                    DateTime.UtcNow.AddHours(1),
                    DateTime.UtcNow.AddDays(1)
                ),
                Booking.Create(
                    dbContext.Contacts.First(x => x.LicenseNumber == "MARV02"),
                    dbContext.Cars.First(x => x.RegistrationNumber == "IRONMAN"),
                    DateTime.UtcNow.AddHours(2),
                    DateTime.UtcNow.AddDays(2)
                ),
                Booking.Create(
                    dbContext.Contacts.First(x => x.LicenseNumber == "MARV03"),
                    dbContext.Cars.First(x => x.RegistrationNumber == "HULK"),
                    DateTime.UtcNow.AddHours(3),
                    DateTime.UtcNow.AddDays(3)
                ),
                Booking.Create(
                    dbContext.Contacts.First(x => x.LicenseNumber == "DC0001"),
                    dbContext.Cars.First(x => x.RegistrationNumber == "SUPERMAN"),
                    DateTime.UtcNow.AddHours(4),
                    DateTime.UtcNow.AddDays(4)
                ),
                Booking.Create(
                    dbContext.Contacts.First(x => x.LicenseNumber == "DC0002"),
                    dbContext.Cars.First(x => x.RegistrationNumber == "BATMOBILE"),
                    DateTime.UtcNow.AddHours(5),
                    DateTime.UtcNow.AddDays(5)
                ),
                Booking.Create(
                    dbContext.Contacts.First(x => x.LicenseNumber == "DC0003"),
                    dbContext.Cars.First(x => x.RegistrationNumber == "LEXCORP"),
                    DateTime.UtcNow.AddHours(6),
                    DateTime.UtcNow.AddDays(6)
                )
            };

            dbContext.Bookings.AddRange(bookings);
            dbContext.SaveChanges();
        }
    }

}