using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Application.Cars;
using Praedico.Bookings.Application.Contacts;
using Praedico.Bookings.Application.Repositories;
using Praedico.Bookings.Application.Schedules;
using Praedico.Bookings.Domain.Cars;
using Praedico.Bookings.Domain.Contacts;
using Praedico.Bookings.Domain.Schedules;
using Praedico.Bookings.Infrastructure.Data;
using Praedico.Bookings.Infrastructure.Diagnostics;
using Praedico.Bookings.Infrastructure.Repositories;
using Serilog;

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
        var dbFilePath = GetOrCreateDbPath();
        services.AddDbContext<BookingsDbContext>(options =>
        {
            options.UseSqlite($"DataSource={dbFilePath}");
            options.EnableSensitiveDataLogging();
            options.LogTo(Log.Information, LogLevel.Information);
        });

        services.AddSingleton<IObserver<DiagnosticListener>, DiagnosticsObserver>();
    }

    private static string GetOrCreateDbPath()
    {
        var dbDirectory = Path.Combine(AppContext.BaseDirectory, ".db");
        if (!Directory.Exists(dbDirectory))
            Directory.CreateDirectory(dbDirectory);

        var dbPath = Path.Combine(dbDirectory, "Bookings.db");
        Log.Information("Database path set to: {DbPath}", dbPath);
        return dbPath;
    }
    
    private static void AddRepositories(this IServiceCollection services)
    {
        // Command repositories
        services.AddScoped<ICommandRepository<Booking>, CommandRepository<Booking>>();
        services.AddScoped<ICommandRepository<Car>, CommandRepository<Car>>();
        services.AddScoped<ICommandRepository<Contact>, CommandRepository<Contact>>();
        services.AddScoped<ICommandRepository<Schedule>, CommandRepository<Schedule>>();
        
        services.AddScoped<IBookingCommandRepository, BookingCommandRepository>();
        services.AddScoped<ICarCommandRepository, CarCommandRepository>();
        services.AddScoped<IContactCommandRepository, ContactCommandRepository>();
        services.AddScoped<IScheduleCommandRepository, ScheduleCommandRepository>();

        // Query repositories
        services.AddScoped<IQueryRepository<Booking>, QueryRepository<Booking>>();
        services.AddScoped<IQueryRepository<Car>, QueryRepository<Car>>();
        services.AddScoped<IQueryRepository<Contact>, QueryRepository<Contact>>();
        services.AddScoped<IQueryRepository<Schedule>, QueryRepository<Schedule>>();
        
        services.AddScoped<IBookingQueryRepository, BookingQueryRepository>();
        services.AddScoped<ICarQueryRepository, CarQueryRepository>();
        services.AddScoped<IContactQueryRepository, ContactQueryRepository>();
        services.AddScoped<IScheduleQueryRepository, ScheduleQueryRepository>();
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