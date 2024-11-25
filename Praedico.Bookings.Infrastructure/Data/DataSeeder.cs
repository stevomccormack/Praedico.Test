using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static void SeedAll(this ModelBuilder modelBuilder)
        {
            SeedCars(modelBuilder);
            SeedContacts(modelBuilder);
            SeedSchedule(modelBuilder);
            SeedBookings(modelBuilder);
        }
        
        public static void SeedCars(this ModelBuilder modelBuilder)
        {
            var cars = SeedHelper.GetCars();
            modelBuilder.Entity<Car>().HasData(cars);
        }
        
        public static void SeedContacts(this ModelBuilder modelBuilder)
        {
            var contacts = SeedHelper.GetContacts();
            modelBuilder.Entity<Contact>().HasData(contacts);
        }
        
        public static void SeedSchedule(this ModelBuilder modelBuilder)
        {
            var schedules = SeedHelper.GetSchedules();
            modelBuilder.Entity<Schedule>().HasData(schedules);
        }
        
        public static void SeedBookings(this ModelBuilder modelBuilder)
        {
            var bookings = SeedHelper.GetBookings();
            modelBuilder.Entity<Booking>().HasData(bookings);
        }
    }

    internal static class SeedHelper
    {
        public static IReadOnlyList<Car> GetCars()
        {
            return new List<Car>
            {
                Car.Create(CarType.Compact, "SPIDERMAN"),
                Car.Create(CarType.Compact, "IRONMAN"),
                Car.Create(CarType.Van, "HULK"),
                Car.Create(CarType.SUV, "SUPERMAN"),
                Car.Create(CarType.Compact, "BATMOBILE"),
                Car.Create(CarType.Compact, "LEXCORP")
            };
        }

        public static IReadOnlyList<Contact> GetContacts()
        {
            return new List<Contact>
            {
                Contact.Create("MARV01", "Peter", "Parker"),
                Contact.Create("MARV02", "Tony", "Stark"),
                Contact.Create("MARV03", "Bruce", "Banner"),
                Contact.Create("DC0001", "Clark", "Kent"),
                Contact.Create("DC0002", "Bruce", "Wayne"),
                Contact.Create("DC0013", "Lex", "Luthor")
            };
        }

        public static IReadOnlyList<Schedule> GetSchedules()
        {
            return new List<Schedule>
            {
                Schedule.Create("ASGARD"),
                Schedule.Create("METROPOLIS")
            };
        }

        public static IReadOnlyList<Booking> GetBookings()
        {
            var contacts = GetContacts();
            var cars = GetCars();

            return new List<Booking>
            {
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
                ),
            };
        }
    }
}