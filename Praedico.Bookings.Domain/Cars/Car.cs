using Praedico.Guards;

namespace Praedico.Bookings.Domain.Cars;

public class Car: Entity
{
    public string RegistrationNumber { get; private set; } //unique
    public CarType CarType { get; private set; }
    public CarHireStatus Status { get; private set; }
    public bool IsActive { get; private set; } = true;

    private Car(Guid id, CarType carType, string registrationNumber): base(id)
    {
        CarType = carType;
        RegistrationNumber = registrationNumber;
        Status = CarHireStatus.Available;
    }

    public static Car Create(CarType carType, string registrationNumber)
    {
        Guard.Against.Null(carType, nameof(carType));
        Guard.Against.NullOrWhiteSpace(registrationNumber, nameof(registrationNumber));
        Guard.Against.InvalidRegistrationNumber(registrationNumber);

        return new Car(Guid.NewGuid(), carType, registrationNumber);
    }

    public void ChangeCarType(CarType carType)
    {
        Guard.Against.Null(carType, nameof(carType));

        CarType = carType;
    }

    public void ChangeRegistrationNumber(string registrationNumber)
    {
        Guard.Against.NullOrEmpty(registrationNumber, nameof(registrationNumber));
        Guard.Against.InvalidRegistrationNumber(registrationNumber);

        RegistrationNumber = registrationNumber;
    }

    public void Available()
    {
        ChangeStatus(CarHireStatus.Available);
    }

    public void Hired()
    {
        ChangeStatus(CarHireStatus.Hired);
    } 

    public void Unavailable()
    {
        ChangeStatus(CarHireStatus.Unavailable);
    }

    private void ChangeStatus(CarHireStatus status)
    {
        if (!status.Equals(Status))
            Status = status;
        
        Status = status;
    }

    public void Activate()
    {
        if (!IsActive)
            IsActive = true;
    }

    public void Deactivate()
    {
        if (IsActive)
            IsActive = false;
    }
}

