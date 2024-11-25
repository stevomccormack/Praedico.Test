using System.Text.RegularExpressions;
using Praedico.Exceptions;
using Praedico.Guards;

namespace Praedico.Bookings.Domain.Cars;

public static class CarGuards
{
    public static void InvalidRegistrationNumber(this IGuardClause guardClause, string registrationNumber)
    {
        if (!Regex.IsMatch(registrationNumber, @"^[a-zA-Z0-9]+$")) // alphanumeric
            throw new BusinessException($"The registration number must be alphanumeric: {registrationNumber}.", 
                "INVALID_CAR_REGISTRATION");
    }
    
    public static void InactiveCar(this IGuardClause guardClause, Car car)
    {
        if (!car.IsActive)
            throw new BusinessException($"Inactive car: {car.RegistrationNumber}.", 
                "INACTIVE_CAR");
    }
}
