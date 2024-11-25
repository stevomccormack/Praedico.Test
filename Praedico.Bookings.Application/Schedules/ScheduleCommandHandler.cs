using Praedico.Bookings.Domain.Schedules;

namespace Praedico.Bookings.Application.Schedules;

public class ScheduleCommandHandler(IScheduleCommandRepository carCommandRepository)
{
    private IScheduleCommandRepository ScheduleCommandRepository{ get; } = carCommandRepository;

    public async Task<Schedule> CreateSchedule(CreateScheduleRequest request, CancellationToken cancellationToken = default)
    {
        var schedule = Schedule.Create(request.LocationCode);
        
        return await ScheduleCommandRepository.CreateAsync(schedule, cancellationToken: cancellationToken);
    }

    public void UpdateSchedule(string licenseNumber, CreateScheduleRequest request, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
    }

    public void DeleteSchedule(string licenseNumber, CancellationToken cancellationToken = default)
    {
        //@TODO: Not implemented
    }
}