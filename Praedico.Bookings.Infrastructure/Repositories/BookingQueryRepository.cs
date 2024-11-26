using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories;

public class BookingQueryRepository : QueryRepository<Booking>, IBookingQueryRepository
{
    public BookingQueryRepository(BookingsDbContext dbContext) : base(dbContext)
    {
        Query = Query
            .Include(x => x.Contact)
            .Include(x => x.Car);
    }

    public async Task<Booking?> GetUniqueAsync(string bookingReference, CancellationToken cancellationToken = default)
    {
        var normalizedReference = bookingReference.ToLower();
        return await Query.FirstOrDefaultAsync(
            x => EF.Functions.Like(x.BookingReference.ToLower(), normalizedReference),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsUniqueAsync(string bookingReference, CancellationToken cancellationToken = default)
    {
        var normalizedReference = bookingReference.ToLower();
        return await Query.AnyAsync(
            x => EF.Functions.Like(x.BookingReference.ToLower(), normalizedReference),
            cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyList<CarType>> CheckCarTypeAvailability(
        DateTime pickupDateTime,
        DateTime returnDateTime,
        string[]? carTypes,
        CancellationToken cancellationToken = default)
    {
        var query = Query.AvailableCars(pickupDateTime, returnDateTime, carTypes);

        return await query
            .Select(x => x.Car.CarType)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Car>> CheckCarAvailability(
        DateTime pickupDateTime,
        DateTime returnDateTime,
        string[]? carTypes,
        CancellationToken cancellationToken = default)
    {
        var query = Query.AvailableCars(pickupDateTime, returnDateTime, carTypes);

        return await query
            .Select(x => x.Car)
            .Distinct()
            .ToListAsync(cancellationToken);
    }

    public new async Task<IEnumerable<Booking>> SearchAsync(
        string searchText,
        Expression<Func<Booking, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var normalizedSearchText = searchText.ToLower();
            Query = Query.Where(b =>
                EF.Functions.Like(b.Contact.FullName.ToLower(), $"%{normalizedSearchText}%") ||
                EF.Functions.Like(b.Car.RegistrationNumber.ToLower(), $"%{normalizedSearchText}%") ||
                EF.Functions.Like(b.BookingReference.ToLower(), $"%{normalizedSearchText}%"));
        }

        if (filter != null)
        {
            Query = Query.Where(filter);
        }

        if (pageNumber.HasValue && pageSize.HasValue)
        {
            Query = Query.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
        }

        return await Query.ToListAsync(cancellationToken);
    }
}
