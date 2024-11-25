using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;

namespace Praedico.Bookings.Api.Bookings;

public static class BookingResponseMaps
{
    public static IReadOnlyList<BookingResponse> ToResponse(this IReadOnlyList<Booking> bookings)
    {
        return bookings.Select(x => x.ToResponse()).ToList().AsReadOnly();
    }

    public static BookingResponse ToResponse(this Booking booking)
    {
        return new BookingResponse
        {
            //BookingId // dont leak domain
            BookingReference = booking.BookingReference,
            PickupDateTime = booking.TimeRange.Start,
            ReturnDateTime = booking.TimeRange.End,
            BookingStatus = booking.Status.Name,
            StatusChangedOn = booking.StatusChangedOn,
            LastModifiedOn = booking.LastModifiedOn,
            CreatedOn = booking.CreatedOn,
            Contact = booking.Contact.ToResponse(),
            Car = booking.Car.ToResponse()
        };
    }

    public static ContactResponse ToResponse(this Contact contact)
    {
        return new ContactResponse
        {
            //ContactId // dont leak domain
            LicenseNumber = contact.LicenseNumber,
            GivenName = contact.GivenName,
            Surname = contact.Surname,
            Email = contact.Email,
            Phone = contact.Phone,
            IsActive = contact.IsActive
        };
    }

    public static CarResponse ToResponse(this Car car)
    {
        return new CarResponse
        {
            //CarId // dont leak domain
            RegistrationNumber = car.RegistrationNumber,
            CarType = car.CarType.Name,
            HireStatus = car.Status.Name
        };
    }
}