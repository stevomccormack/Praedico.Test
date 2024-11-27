using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Api.Bookings;

public static class BookingRequestMap
{
    public static Booking ToDomain(this BookingRequest request)
    {
        var contact = Contact.Create(request.ContactLicenseNumber, request.ContactGivenName, request.ContactSurname);
        var carType = request.CarType;
        var car = Car.Create(request.CarRegistrationNumber, carType);
        return Booking.Create(contact, car, request.PickupDateTime, request.ReturnDateTime);
    }
}