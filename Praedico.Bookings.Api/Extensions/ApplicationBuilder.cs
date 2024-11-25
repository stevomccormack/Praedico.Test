namespace Praedico.Bookings.Api.Extensions;

public static class ApplicationBuilder
{
    public static void ConfigureMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {    
            app.UseDeveloperExceptionPage();

            //app.MapOpenApi();
            //app.UseSwagger();
            //app.UseSwaggerUI();
        }
        else
        {
            app.UseHttpsRedirection();
        }
    }
}
