using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Application.Contacts;
using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Application.Schedules;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;
using Praedico.Bookings.Infrastructure.Repositories;

namespace Praedico.Bookings.Infrastructure;

public static class ServiceBuilder
{    
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddDataContext();
        services.AddRepositories();
        services.AddIntegrationEvents();
    }

    private static void AddDataContext(this IServiceCollection services)
    {
        services.AddDbContext<BookingsDbContext>(options =>
            {
                options.UseSqlite("DataSource=:memory:");
                options.EnableSensitiveDataLogging();
            }, 
            contextLifetime: ServiceLifetime.Scoped, 
            optionsLifetime: ServiceLifetime.Singleton);

        services.AddScoped(provider =>
        {
            var context = provider.GetRequiredService<BookingsDbContext>();
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        });

        services.AddDbContextFactory<BookingsDbContext>(options =>
            options.UseSqlite("DataSource=:memory:"));
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        // Command repositories (aggregates - mutation through aggregates only)
        services.AddTransient<ICommandRepository<Booking>, CommandRepository<Booking>>();
        services.AddTransient<ICommandRepository<Car>, CommandRepository<Car>>();
        services.AddTransient<ICommandRepository<Contact>, CommandRepository<Contact>>();
        services.AddTransient<ICommandRepository<Schedule>, CommandRepository<Schedule>>();
        
        services.AddTransient<IBookingCommandRepository, BookingCommandRepository>();
        services.AddTransient<ICarCommandRepository, CarCommandRepository>();
        services.AddTransient<IContactCommandRepository, ContactCommandRepository>();
        services.AddTransient<IScheduleCommandRepository, ScheduleCommandRepository>();

        // Query repositories
        services.AddTransient<IQueryRepository<Booking>, QueryRepository<Booking>>();
        services.AddTransient<IQueryRepository<Car>, QueryRepository<Car>>();
        services.AddTransient<IQueryRepository<Contact>, QueryRepository<Contact>>();
        services.AddTransient<IQueryRepository<Schedule>, QueryRepository<Schedule>>();
        
        services.AddTransient<IBookingQueryRepository, BookingQueryRepository>();
        services.AddTransient<ICarQueryRepository, CarQueryRepository>();
        services.AddTransient<IContactQueryRepository, ContactQueryRepository>();
        services.AddTransient<IScheduleQueryRepository, ScheduleQueryRepository>();
    }
    
    private static void AddIntegrationEvents(this IServiceCollection services)
    {
        // BookingPlacedEvent
        // BookingConfirmedEvent
        // BookingAbandonedEvent
        // BookingCancelledEvent
        // BookingRescheduledEvent        
    }
}