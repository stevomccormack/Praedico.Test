using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Praedico.Bookings.Application.Contacts;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Infrastructure.Data;

namespace Praedico.Bookings.Infrastructure.Repositories;

public class ContactQueryRepository(BookingsDbContext dbContext)
    : QueryRepository<Contact>(dbContext), IContactQueryRepository
{
    public async Task<Contact?> GetUniqueAsync(string licenseNumber, CancellationToken cancellationToken = default)
    {
        var normalized = licenseNumber.ToLower();
        return await Query.FirstOrDefaultAsync(
            x => EF.Functions.Like(x.LicenseNumber.ToLower(), normalized),
            cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsUniqueAsync(string licenseNumber, CancellationToken cancellationToken = default)
    {
        var normalized = licenseNumber.ToLower();
        return await Query.AnyAsync(
            x => EF.Functions.Like(x.LicenseNumber.ToLower(), normalized),
            cancellationToken: cancellationToken);
    }

    public new async Task<IEnumerable<Contact>> SearchAsync(
        string searchText,
        Expression<Func<Contact, bool>>? filter = null,
        int? pageNumber = null,
        int? pageSize = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            var normalizedSearchText = searchText.ToLower();
            Query = Query.Where(b =>
                EF.Functions.Like(b.LicenseNumber.ToLower(), $"%{normalizedSearchText}%") &&
                EF.Functions.Like(b.FullName.ToLower(), $"%{normalizedSearchText}%"));
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
