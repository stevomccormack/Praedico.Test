namespace Praedico.Exceptions;

/// <summary>
/// Business Exception - distinguish business rule violations
/// </summary>
public class BusinessException : Exception
{
    public string? Code { get; }

    public BusinessException(string message, string? code = "BUSINESS_RULE_VIOLATION") : 
        base(message)
    {
        Code = code;
    }
    
    //InnerException
}