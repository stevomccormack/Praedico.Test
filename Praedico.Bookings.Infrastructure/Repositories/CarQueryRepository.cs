using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories;

public class CarQueryRepository : QueryRepository<Car>, ICarQueryRepository
{
    public CarQueryRepository(BookingsDbContext dbContext) : base(dbContext)
    {
        
    }

    public async Task<Car?> GetUniqueAsync(string registrationNumber, CancellationToken cancellationToken = default)
    {
        var normalized = registrationNumber.ToLower();
        return await Query.FirstOrDefaultAsync(
            x => EF.Functions.Like(x.RegistrationNumber.ToLower(), normalized),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsUniqueAsync(string registrationNumber, CancellationToken cancellationToken = default)
    {
        var normalized = registrationNumber.ToLower();
        return await Query.AnyAsync(
            x => EF.Functions.Like(x.RegistrationNumber.ToLower(), normalized),
            cancellationToken: cancellationToken);
    }

    public new async Task<IEnumerable<Car>> SearchAsync(
        string searchText,
        Expression<Func<Car, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var normalizedSearchText = searchText.ToLower();
            Query = Query.Where(b =>
                EF.Functions.Like(b.RegistrationNumber.ToLower(), $"%{normalizedSearchText}%"));
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
