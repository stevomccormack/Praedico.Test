using System.Diagnostics.CodeAnalysis;
using Praedico.Exceptions;

namespace Praedico.Guards;

/// <summary>
/// Guard clauses - based on Steve Ardalis Guards
/// </summary>
public static class Guards
{
    public static void Null<T>(this IGuardClause guardClause, [DisallowNull][NotNull] T value, string parameterName)
    {
        if (value is null)
            throw new BusinessException($"The parameter '{parameterName}' cannot be null.", "NULL_PARAMETER");
    }

    public static void NullOrEmpty(this IGuardClause guardClause, string value, string parameterName)
    {
        guardClause.Null(value, parameterName);
        if (value.Equals(string.Empty))
            throw new BusinessException($"The parameter '{parameterName}' cannot be empty.", "EMPTY_PARAMETER");
    }

    public static void NullOrEmpty(this IGuardClause guardClause, [DisallowNull][NotNull] Guid? value, string parameterName)
    {
        guardClause.Null(value, parameterName);
        if (value.Equals(Guid.Empty))
            throw new BusinessException($"The parameter '{parameterName}' cannot be empty.", "EMPTY_PARAMETER");
    }

    public static void NullOrEmpty<T>(this IGuardClause guardClause, IEnumerable<T> value, string parameterName)
    {
        var list = value.ToList();
        guardClause.Null(list, parameterName);
        if (list.Count == 0)
            throw new BusinessException($"The parameter '{parameterName}' cannot be empty.", "EMPTY_PARAMETER");
    }

    public static void NullOrWhiteSpace(this IGuardClause guardClause, string value, string parameterName)
    {
        guardClause.NullOrEmpty(value, parameterName);
        if (string.IsNullOrWhiteSpace(value))
            throw new BusinessException($"The parameter '{parameterName}' cannot be empty or whitespace.", "NULL_OR_WHITESPACE_PARAMETER");
    }

    public static void Zero<T>(this IGuardClause guardClause, T value, string parameterName) where T : struct
    {
        if (EqualityComparer<T>.Default.Equals(value, default))
            throw new BusinessException($"The parameter '{parameterName}' cannot be zero.", "INVALID_CANNOT_BE_ZERO");
    }

    public static void Negative<T>(this IGuardClause guardClause, T value, string parameterName) where T : struct, IComparable
    {
        if (value.CompareTo(default) < 0)
            throw new BusinessException($"The parameter '{parameterName}' cannot be negative.", "INVALID_CANNOT_BE_NEGATIVE");
    }

    public static void NegativeOrZero<T>(this IGuardClause guardClause, T value, string parameterName) where T : struct, IComparable
    {
        if (value.CompareTo(default) <= 0)
            throw new BusinessException($"The parameter '{parameterName}' cannot be zero or negative.", "INVALID_CANNOT_BE_NEGATIVE");
    }
    
    public static void InvalidEmail(this IGuardClause guardClause, string email)
    {
        //@TODO: validate email
    }
    
    public static void InvalidPhone(this IGuardClause guardClause, string phone)
    {
        //@TODO: validate phone
    }
}
