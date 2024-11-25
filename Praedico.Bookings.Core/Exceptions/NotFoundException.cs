namespace Praedico.Exceptions;

/// <summary>
/// Not Found Exception - distinguish business rule or not found.
/// </summary>
public class NotFoundException(string message, string? code = "NOT_FOUND") : BusinessException(message, code);