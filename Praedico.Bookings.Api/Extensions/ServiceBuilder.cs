using Praedico.Bookings.Application;
using Praedico.Bookings.Infrastructure;
using Praedico.Bookings.Api.Bookings;

namespace Praedico.Bookings.Api.Extensions;

public static class ServiceBuilder
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure();        
        services.AddApi();
        
        //services.AddOpenApi();
        //services.AddEndpointsApiExplorer();
        //services.AddSwaggerGen();
        
    }
    
    private static void AddApi(this IServiceCollection services)
    {
        services.AddScoped<BookingApiCommandHandler>();  
        services.AddScoped<BookingApiQueryHandler>();
    }

}
