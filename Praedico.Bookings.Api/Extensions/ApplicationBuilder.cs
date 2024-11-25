using Praedico.Bookings.Api.Bookings;
using Praedico.Bookings.Application.Bookings;
using Praedico.Bookings.Infrastructure.Data;
using Serilog;

namespace Praedico.Bookings.Api.Extensions;

public static class ApplicationBuilder
{
    public static void ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {    
            app.UseApplicationLifetimeEventLogging();
            app.UseDeveloperExceptionPage();
            app.UseSerilogRequestLogging();

            //app.MapOpenApi();
            //app.UseSwagger();
            //app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts(); 
            app.UseHttpsRedirection();
        }
        
        app.UseRouting(); // Must be before endpoint mappings
        //app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        app.UseBookingEndpoints();
        if (app.Environment.IsDevelopment()) app.UseTestEndpoints();

        //app.UseAuthorization();
        app.UseSerilogCloseAndFlushOnStopped();
    }

    public static void UseSerilog(this WebApplicationBuilder builder)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();
    }

    private static void UseTestEndpoints(this WebApplication app)
    {
        app.MapGet("/", () => "Praedico Booking Api - Running...");
        
        app.MapGet("/test", () => "Test works!");
        
        app.MapGet("/test-dbcontext", (IServiceProvider provider) =>
        {
            var dbContext = provider.GetService<BookingsDbContext>();
            return dbContext != null ? Results.Ok("BookingsDbContext resolved") : Results.Problem("Failed to resolve BookingsDbContext");
        });

        app.MapGet("/test-repository", (IServiceProvider provider) =>
        {
            var queryRepository = provider.GetService<IBookingQueryRepository>();
            return queryRepository != null ? Results.Ok("QueryRepository resolved") : Results.Problem("Failed to resolve QueryRepository");
        });
        
        app.MapGet("/test-handler", (IServiceProvider provider) =>
        {
            var queryHandler = provider.GetService<BookingQueryHandler>();
            return queryHandler != null ? Results.Ok("BookingQueryHandler resolved") : Results.Problem("Failed to resolve BookingQueryHandler");
        });
        
        app.MapGet("/test-api-handler", (IServiceProvider provider) =>
        {
            var queryHandler = provider.GetService<BookingApiQueryHandler>();
            return queryHandler != null ? Results.Ok("BookingApiQueryHandler resolved") : Results.Problem("Failed to resolve BookingApiQueryHandler");
        });
    }

    private static void UseApplicationLifetimeEventLogging(this WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(() => Log.Information("Application started"));
        app.Lifetime.ApplicationStopped.Register(() => Log.Information("Application stopped"));
    }

    private static void UseSerilogCloseAndFlushOnStopped(this WebApplication app)
    {
        app.Lifetime.ApplicationStopped.Register(Log.CloseAndFlush);
    }
}
