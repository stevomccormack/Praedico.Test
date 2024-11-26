using Praedico.Bookings.Domain;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Application.Bookings;

    public static class BookingQueryableExtensions
    {
        public static IQueryable<Booking> OverlapsWith(this IQueryable<Booking> query, DateTimeRange range)
        {
            return query.Where(x => 
                x.TimeRange.Start < range.End && 
                x.TimeRange.End > range.Start);
        }

        public static IQueryable<Booking> IsWithin(this IQueryable<Booking> query, DateTimeRange range)
        {
            return query.Where(x => 
                x.TimeRange.Start <= range.Start && 
                x.TimeRange.End >= range.End);
        }

        public static IQueryable<Booking> CollidesWith(this IQueryable<Booking> query, DateTimeRange range)
        {
            return query.Where(x =>
                (x.TimeRange.Start <= range.Start && x.TimeRange.End >= range.End) || // StartsBeforeAndEndsAfter
                (x.TimeRange.Start <= range.Start && x.TimeRange.End > range.Start && x.TimeRange.End <= range.End) || // StartsBeforeAndEndsDuring
                (x.TimeRange.Start >= range.Start && x.TimeRange.Start < range.End && x.TimeRange.End >= range.End) || // StartsDuringAndEndsAfter
                (x.TimeRange.Start >= range.Start && x.TimeRange.End <= range.End)); // IsWithin
        }

        public static IQueryable<Booking> AvailableCars(this IQueryable<Booking> query, 
            DateTime pickupDateTime,
            DateTime returnDateTime,
            string[]? carTypes)
        {
            query = query.Where(x => x.Car.IsActive);

            // Apply car type filter if specified
            if (carTypes?.Length > 0){
                var carTypesList = carTypes.ToList();
                query = query.Where(x => carTypesList.Any(carType => carType == x.Car.CarType.Name));
            }

            // Exclude cars that collide with the provided time range
            query = query.Where(x =>
                !(x.PickupDateTime <= pickupDateTime && x.ReturnDateTime >= returnDateTime) && // Not StartsBeforeAndEndsAfter
                !(x.PickupDateTime <= pickupDateTime && x.ReturnDateTime > pickupDateTime && x.ReturnDateTime <= returnDateTime) && // Not StartsBeforeAndEndsDuring
                !(x.PickupDateTime >= pickupDateTime && x.PickupDateTime < returnDateTime && x.ReturnDateTime >= returnDateTime) && // Not StartsDuringAndEndsAfter
                !(x.PickupDateTime >= pickupDateTime && x.ReturnDateTime <= returnDateTime)); // Not IsWithin

            return query;
        }
    }

