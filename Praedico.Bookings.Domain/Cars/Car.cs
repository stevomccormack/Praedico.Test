using Praedico.Guards;

namespace Praedico.Bookings.Domain.Cars;

public class Car: Entity
{
    public string RegistrationNumber { get; private set; } //unique
    public CarType CarType { get; private set; }
    public CarStatus Status { get; private set; } = CarStatus.Available;
    public bool IsActive { get; private set; } = true;

    private Car(Guid id, string registrationNumber, CarType carType): base(id)
    {
        RegistrationNumber = registrationNumber;
        CarType = carType;
    }

    public static Car Create(string registrationNumber, CarType carType)
    {
        Guard.Against.NullOrWhiteSpace(registrationNumber, nameof(registrationNumber));
        Guard.Against.InvalidRegistrationNumber(registrationNumber);
        Guard.Against.Null(carType, nameof(carType));

        return new Car(Guid.NewGuid(), registrationNumber, carType);
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
        ChangeStatus(CarStatus.Available);
    }

    public void Unavailable()
    {
        ChangeStatus(CarStatus.Unavailable);
    }

    private void ChangeStatus(CarStatus status)
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

