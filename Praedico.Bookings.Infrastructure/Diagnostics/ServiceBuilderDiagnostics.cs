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
using Serilog;

namespace Praedico.Bookings.Infrastructure.Diagnostics;

public static class ServiceBuilderDiagnostics
{
    /// <summary>
    /// Validates the database path and dependency injection setup for the infrastructure layer.
    /// </summary>
    public static void ValidateInfrastructure(this IServiceProvider serviceProvider)
    {
        Log.Information("Starting Infrastructure Diagnostics...");

        // Validate database path
        ValidateDbPath();

        // Validate dependency injection
        ValidateDependencyInjection(serviceProvider);

        Log.Information("Infrastructure Diagnostics completed.");
    }

    /// <summary>
    /// Validates the database path and ensures it exists.
    /// </summary>
    private static void ValidateDbPath()
    {
        try
        {
            var dbDirectory = Path.Combine(AppContext.BaseDirectory, ".db");
            var dbPath = Path.Combine(dbDirectory, "Bookings.db");

            if (!Directory.Exists(dbDirectory))
            {
                Log.Warning("Database directory does not exist: {DbDirectory}", dbDirectory);
            }

            if (!File.Exists(dbPath))
            {
                Log.Warning("Database file does not exist: {DbPath}", dbPath);
            }
            else
            {
                Log.Information("Database file found at: {DbPath}", dbPath);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while validating database path.");
        }
    }

    /// <summary>
    /// Validates that all required services are registered in the DI container.
    /// </summary>
    private static void ValidateDependencyInjection(IServiceProvider serviceProvider)
    {

        using var scope = serviceProvider.CreateScope(); // Create a scope
        var scopedProvider = scope.ServiceProvider;
        
        try
        {
            Log.Information("Validating Dependency Injection...");

            // Add checks for required services
            CheckServiceRegistration<BookingsDbContext>(scopedProvider);
            
            CheckServiceRegistration<ICommandRepository<Booking>>(scopedProvider);
            CheckServiceRegistration<ICommandRepository<Car>>(scopedProvider);
            CheckServiceRegistration<ICommandRepository<Contact>>(scopedProvider);
            CheckServiceRegistration<ICommandRepository<Schedule>>(scopedProvider);
            CheckServiceRegistration<IQueryRepository<Booking>>(scopedProvider);
            CheckServiceRegistration<IQueryRepository<Car>>(scopedProvider);
            CheckServiceRegistration<IQueryRepository<Contact>>(scopedProvider);
            CheckServiceRegistration<IQueryRepository<Schedule>>(scopedProvider);
            
            CheckServiceRegistration<IBookingCommandRepository>(scopedProvider);
            CheckServiceRegistration<ICarCommandRepository>(scopedProvider);
            CheckServiceRegistration<IContactCommandRepository>(scopedProvider);
            CheckServiceRegistration<IScheduleCommandRepository>(scopedProvider);
            CheckServiceRegistration<IBookingQueryRepository>(scopedProvider);
            CheckServiceRegistration<ICarQueryRepository>(scopedProvider);
            CheckServiceRegistration<IContactQueryRepository>(scopedProvider);
            CheckServiceRegistration<IScheduleQueryRepository>(scopedProvider);

            Log.Information("Dependency Injection validation completed.");
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error while validating Dependency Injection.");
        }
    }

    /// <summary>
    /// Checks if a specific service is registered in the DI container and logs the result.
    /// </summary>
    private static void CheckServiceRegistration<T>(IServiceProvider serviceProvider)
    {
        var service = serviceProvider.GetService<T>();
        if (service == null)
        {
            Log.Error("Service {ServiceType} is not registered in the DI container.", typeof(T).FullName);
        }
        else
        {
            Log.Information("Service {ServiceType} is registered and resolved successfully.", typeof(T).FullName);
        }
    }
}
