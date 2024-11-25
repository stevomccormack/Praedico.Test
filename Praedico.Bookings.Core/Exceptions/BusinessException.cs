namespace Praedico.Exceptions;

/// <summary>
/// Business Exception - distinguish business rule violations
/// </summary>
public class BusinessException(string message, string? code = "BUSINESS_RULE_VIOLATION") : Exception(message)
{
    public string? Code { get; } = code;

    //InnerException
}