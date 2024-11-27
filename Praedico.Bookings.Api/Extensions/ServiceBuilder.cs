using System.Text.Json;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Praedico.Bookings.Application;
using Praedico.Bookings.Infrastructure;
using Praedico.Bookings.Api.Bookings;
using Praedico.Bookings.Api.Cars;
using Praedico.Bookings.Api.Contacts;
using Praedico.Bookings.Api.Converters;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;

namespace Praedico.Bookings.Api.Extensions;

public static class ServiceBuilder
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddApplication();
        services.AddInfrastructure();        
        services.AddApi();
    }
    
    public static void AddApi(this IServiceCollection services)
    { 
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1",new OpenApiInfo
            {
                Title = "Praedico Bookings API",
                Version = "1.0",
                Description = "Car Hire Bookings API"
            });
            
            options.MapType<DateTime>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date-time",
                Default = new OpenApiString(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ"))
            });
        });
        
        services.AddJsonConverters();
        services.AddApiHandlers();
    }
    
    private static void AddApiHandlers(this IServiceCollection services)
    {
        services.AddScoped<BookingApiCommandHandler>();  
        services.AddScoped<ContactApiCommandHandler>();  
        services.AddScoped<CarApiCommandHandler>(); 
        
        services.AddScoped<BookingApiQueryHandler>();
        services.AddScoped<ContactApiQueryHandler>();
        services.AddScoped<CarApiQueryHandler>();
    }
    
    private static void AddJsonConverters(this IServiceCollection services)
    {
        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new StringConverter());
            options.SerializerOptions.Converters.Add(new DateTimeConverter());
            options.SerializerOptions.Converters.Add(
                new System.Text.Json.Serialization.JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false));
        });
    }

}
