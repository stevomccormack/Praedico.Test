using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Domain;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Api.Bookings;

public static class BookingRequestMap
{
    public static Booking ToDomain(this CreateBookingRequest request)
    {
        var contact = Contact.Create(request.ContactLicenseNumber, request.ContactGivenName, request.ContactSurname);
        var carType = Enumeration.FromName<CarType>(request.CarType);
        var car = Car.Create(carType, request.CarRegistrationNumber);
        return Booking.Create(contact, car, request.PickupDateTime, request.ReturnDateTime);
    }

    // public static Contact ToDomain(this ContactRequest request)
    // {
    //     var contact = Contact.Create(request.LicenseNumber, request.GivenName, request.Surname);
    //     contact.SetEmail(request.Email); 
    //     contact.SetPhone(request.Phone);
    //     return contact;
    // }
    //
    // public static Car ToDomain(this CarRequest request)
    // {
    //     var carType = Enumeration.FromName<CarType>(request.CarType);
    //     var hireStatus = Enumeration.FromName<CarHireStatus>(request.HireStatus);
    //     
    //     var car = Car.Create(carType, request.RegistrationNumber);
    //     if (hireStatus.Equals(CarHireStatus.Hired))
    //         car.Hired();
    //     if (hireStatus.Equals(CarHireStatus.Unavailable))
    //         car.Unavailable();
    //     if (hireStatus.Equals(CarHireStatus.Available))
    //         car.Available();
    //
    //     return car;
    // }
}