using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Application.Schedules;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories;

public class ScheduleQueryRepository : QueryRepository<Schedule>, IScheduleQueryRepository
{
    public ScheduleQueryRepository(BookingsDbContext dbContext) : base(dbContext)
    {
        Query = Query
            .Include(b => b.Bookings);
    }

    public async Task<Schedule?> GetUniqueAsync(string locationCode, CancellationToken cancellationToken = default)
    {
        var normalizedReference = locationCode.ToLower();
        return await Query.FirstOrDefaultAsync(
            x => EF.Functions.Like(x.LocationCode.ToLower(), normalizedReference),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsUniqueAsync(string locationCode, CancellationToken cancellationToken = default)
    {
        var normalizedReference = locationCode.ToLower();
        return await Query.AnyAsync(
            x => EF.Functions.Like(x.LocationCode.ToLower(), normalizedReference),
            cancellationToken: cancellationToken);
    }

    public new async Task<IEnumerable<Schedule>> SearchAsync(
        string searchText,
        Expression<Func<Schedule, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var normalizedSearchText = searchText.ToLower();
            Query = Query.Where(b =>
                EF.Functions.Like(b.LocationCode.ToLower(), $"%{normalizedSearchText}%"));
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
