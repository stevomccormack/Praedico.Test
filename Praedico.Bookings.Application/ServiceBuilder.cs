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
        services.AddTransient<BookingCommandHandler>();        
        services.AddTransient<CarCommandHandler>();        
        services.AddTransient<ContactCommandHandler>();        
        services.AddTransient<ScheduleCommandHandler>();
        
        services.AddTransient<BookingQueryHandler>();
        services.AddTransient<CarQueryHandler>();
        services.AddTransient<ContactQueryHandler>();
        services.AddTransient<ScheduleQueryHandler>();
    }
}