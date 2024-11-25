namespace Praedico.Bookings.Application.Schedules;

public record CreateScheduleRequest
{
    public string LocationCode { get; init; } = string.Empty;
}