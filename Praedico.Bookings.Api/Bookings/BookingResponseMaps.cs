using Praedico.Bookings.Api.Cars;
using Praedico.Bookings.Api.Contacts;
using Praedico.Bookings.Domain.Schedules;

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
}