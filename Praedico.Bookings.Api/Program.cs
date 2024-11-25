using Praedico.Bookings.Api.Bookings;
using Praedico.Bookings.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureServices();

var app = builder.Build();
app.ConfigureMiddleware();
app.ConfigureBookingEndpoints();

app.Run();