using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Application.Contacts;
using Praedico.Bookings.Domain;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Application.Bookings;

public class BookingCommandHandler(
    IBookingCommandRepository bookingCommandRepository,
    ICarQueryRepository carQueryRepository,
    IContactQueryRepository contactQueryRepository)
{
    private IBookingCommandRepository BookingCommandRepository{ get; } = bookingCommandRepository;
    private ICarQueryRepository CarQueryRepository{ get; } = carQueryRepository;
    private IContactQueryRepository ContactQueryRepository{ get; } = contactQueryRepository;

    public async Task<Booking> CreateBooking(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        Contact contact;
        Car car;
        var contactExists = await ContactQueryRepository.ExistsUniqueAsync(request.ContactLicenseNumber, cancellationToken);
        if (contactExists)
        {
            contact = (await ContactQueryRepository.GetUniqueAsync(request.ContactLicenseNumber, cancellationToken))!;
        }
        else
        {
            contact = Contact.Create(request.ContactLicenseNumber, request.ContactGivenName, request.ContactSurname);
        }
        
        var carExists = await CarQueryRepository.ExistsUniqueAsync(request.CarRegistrationNumber, cancellationToken);
        if (carExists)
        {
            car = (await CarQueryRepository.GetUniqueAsync(request.CarRegistrationNumber, cancellationToken))!;
        }
        else
        {
            var carType = Enumeration.FromName<CarType>(request.CarType);
            car = Car.Create(carType, request.CarRegistrationNumber);
        }

        var booking = Booking.Create(contact, car, request.PickupDateTime, request.ReturnDateTime);
        
        return await BookingCommandRepository.CreateAsync(booking, cancellationToken: cancellationToken);
    }

    public void UpdateBooking(string bookingReference, CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
    }

    public void DeleteBooking(string bookingReference, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
    }
}