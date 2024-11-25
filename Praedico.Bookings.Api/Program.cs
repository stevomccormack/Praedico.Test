using Praedico.Bookings.Api.Extensions;
using Praedico.Bookings.Infrastructure.Diagnostics;

var builder = WebApplication.CreateBuilder(args);
builder.UseSerilog();
builder.Services.ConfigureServices();

var app = builder.Build();
app.ConfigureMiddleware();
//app.Services.ValidateInfrastructure(); //@TODO: Debug only! Remove

app.Run();