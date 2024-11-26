using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Application.Contacts;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Exceptions;

namespace Praedico.Bookings.Application.Bookings;

public class BookingCommandHandler(
    IBookingCommandRepository bookingCommandRepository,
    IBookingQueryRepository bookingQueryRepository,
    ICarQueryRepository carQueryRepository,
    IContactQueryRepository contactQueryRepository)
{
    private IBookingCommandRepository BookingCommandRepository{ get; } = bookingCommandRepository;
    private IBookingQueryRepository BookingQueryRepository{ get; } = bookingQueryRepository;
    private ICarQueryRepository CarQueryRepository{ get; } = carQueryRepository;
    private IContactQueryRepository ContactQueryRepository{ get; } = contactQueryRepository;

    public async Task<Booking> CreateBooking(CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        //check availability
        var availableCarTypes = await BookingQueryRepository.CheckCarTypeAvailability(request.PickupDateTime, request.ReturnDateTime,
            [request.CarType]
            , cancellationToken);
        if (availableCarTypes.All(x => x.Name != request.CarType))
            throw new NotFoundException($"Booking unavailable for {request.CarType}.");

        var availableCars = await BookingQueryRepository.CheckCarTypeAvailability(request.PickupDateTime, request.ReturnDateTime,
            [request.CarType]
            , cancellationToken);
        if (availableCars.All(x => x.Name != request.CarType))
            throw new NotFoundException($"Booking unavailable for {request.CarType}.");
        
        // check car
        var carExists = await CarQueryRepository.ExistsUniqueAsync(request.CarRegistrationNumber, cancellationToken);
        if (!carExists)
            throw new NotFoundException($"Booking unavailable for car license plate: {request.CarRegistrationNumber}.");
        var car = (await CarQueryRepository.GetUniqueAsync(request.CarRegistrationNumber, cancellationToken))!;
        
        // find or create contact
        Contact contact;
        var contactExists = await ContactQueryRepository.ExistsUniqueAsync(request.ContactLicenseNumber, cancellationToken);
        if (contactExists)
        {
            contact = (await ContactQueryRepository.GetUniqueAsync(request.ContactLicenseNumber, cancellationToken))!;
        }
        else
        {
            contact = Contact.Create(request.ContactLicenseNumber, request.ContactGivenName, request.ContactSurname);
        }

        // create booking
        var booking = Booking.Create(contact, car, request.PickupDateTime, request.ReturnDateTime);
        
        return await BookingCommandRepository.CreateAsync(booking, cancellationToken: cancellationToken);
    }

    public async Task UpdateBooking(string bookingReference, CreateBookingRequest request, CancellationToken cancellationToken = default)
    {
        if(!await BookingQueryRepository.ExistsUniqueAsync(bookingReference, cancellationToken))
            throw new NotFoundException($"No booking found for {bookingReference}.");
        
        var booking = await BookingQueryRepository.GetUniqueAsync(bookingReference, cancellationToken);
        if (booking == null)
            throw new NotFoundException($"No booking found for {bookingReference}.");
        
        booking.ReSchedule(request.PickupDateTime, request.ReturnDateTime);
        
        // car
        var carExists = await CarQueryRepository.ExistsUniqueAsync(request.CarRegistrationNumber, cancellationToken);
        if (!carExists)
            throw new NotFoundException($"Booking unavailable for car license plate: {request.CarRegistrationNumber}.");
        var car = (await CarQueryRepository.GetUniqueAsync(request.CarRegistrationNumber, cancellationToken))!;
        
        // find or create contact
        Contact contact;
        var contactExists = await ContactQueryRepository.ExistsUniqueAsync(request.ContactLicenseNumber, cancellationToken);
        if (contactExists)
        {
            contact = (await ContactQueryRepository.GetUniqueAsync(request.ContactLicenseNumber, cancellationToken))!;
            contact.ChangeFullName(request.ContactGivenName, request.ContactSurname);
            //email
            //phone
        }
        else
        {
            contact = Contact.Create(request.ContactLicenseNumber, request.ContactGivenName, request.ContactSurname);
            booking.ChangeContact(contact);
        }

        await BookingCommandRepository.UpdateAsync(booking, cancellationToken: cancellationToken);
    }

    public async Task DeleteBooking(string bookingReference, CancellationToken cancellationToken = default)
    {
        await BookingCommandRepository.DeleteUniqueAsync(bookingReference, cancellationToken);
    }
}