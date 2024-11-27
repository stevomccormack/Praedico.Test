using Praedico.Bookings.Api.Bookings;
using Praedico.Bookings.Api.Cars;
using Praedico.Bookings.Api.Contacts;
using Serilog;

namespace Praedico.Bookings.Api.Extensions;

public static class ApplicationBuilder
{
    public static void ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {    
            app.UseDeveloperExceptionPage();
            app.UseApplicationLifetimeEventLogging();
            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseHsts(); 
            app.UseHttpsRedirection();
        }

        app.UseRouting(); // Must be before endpoint mappings
        //app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
        
        app.UseBookingEndpoints();
        app.UseCarEndpoints();
        app.UseContactEndpoints();
        
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
