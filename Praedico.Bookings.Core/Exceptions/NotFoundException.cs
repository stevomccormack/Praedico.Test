namespace Praedico.Exceptions;

/// <summary>
/// Not Found Exception - distinguish business rule or not found.
/// </summary>
public class NotFoundException : BusinessException
{
    public NotFoundException(string message, string? code = "NOT_FOUND") : 
        base(message, code)
    {
        
    }
}