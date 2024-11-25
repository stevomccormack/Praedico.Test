using Praedico.Exceptions;
using Praedico.Guards;

namespace Praedico.Bookings.Domain;

/// <summary>
/// Guard clauses - based on Steve Ardalis Guards
/// </summary>
public static class Guards
{
    public static void StartAfterEnd(this IGuardClause guardClause, DateTime start, DateTime end, string startParameterName, string endParameterName)
    {
        guardClause.Null(start, startParameterName);
        guardClause.Null(end, endParameterName);
        if (start > end)
            throw new BusinessException($"The start time '{startParameterName}' cannot be after the end time '{endParameterName}'.", "INVALID_DATE_RANGE");
    }
}
