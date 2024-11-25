﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Domain;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories;

public class BookingQueryRepository : QueryRepository<Booking>, IBookingQueryRepository
{
    public BookingQueryRepository(BookingsDbContext dbContext) : base(dbContext)
    {
        Query = Query
            .Include(b => b.Contact)
            .Include(b => b.Car);
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

    public async Task<bool> CheckAvailabilityAsync(
        DateTime pickupDateTime,
        DateTime returnDateTime,
        string[]? carTypes,
        CancellationToken cancellationToken = default)
    {
        var timeRange = DateTimeRange.Create(pickupDateTime, returnDateTime);
        var availableStatus = CarHireStatus.Available.Name;
        var considerAllCarTypes = carTypes == null || carTypes.Length == 0;        
        
        // Check availability
        if (considerAllCarTypes)
        {
            return await Query.AnyAsync(
                x =>
                    !x.TimeRange.Overlaps(timeRange) && // Ensure no time overlap
                    x.Car.Status.Name == availableStatus && // Car must be available
                    x.Car.IsActive, // Car must be active
                cancellationToken);
        }

        return await Query.AnyAsync(
            x =>
                !x.TimeRange.Overlaps(timeRange) && // Ensure no time overlap
                x.Car.Status.Name == availableStatus && // Car must be available
                carTypes!.Contains(x.Car.CarType.Name) && // Match car types
                x.Car.IsActive, // Car must be active
            cancellationToken);
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