using Microsoft.Extensions.DependencyInjection;
using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Application.Contacts;
using Praedico.Bookings.Application.Schedules;

namespace Praedico.Bookings.Application;

public static class ServiceBuilder
{    
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddHandlers();
    }
    
    private static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<BookingCommandHandler>();        
        services.AddScoped<CarCommandHandler>();        
        services.AddScoped<ContactCommandHandler>();        
        services.AddScoped<ScheduleCommandHandler>();
        
        services.AddScoped<BookingQueryHandler>();
        services.AddScoped<CarQueryHandler>();
        services.AddScoped<ContactQueryHandler>();
        services.AddScoped<ScheduleQueryHandler>();
    }
}