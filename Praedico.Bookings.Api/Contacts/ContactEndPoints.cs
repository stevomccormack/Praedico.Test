using Praedico.Bookings.Application.Contacts;

namespace Praedico.Bookings.Api.Contacts;

public static class ContactEndpoints
{
    public static void UseContactEndpoints(this WebApplication app)
    {
        var apiGroup = app.MapGroup("/api/contacts").WithTags("Contacts");
        app.MapContactCommandEndpoints(apiGroup);
        app.MapContactQueryEndpoints(apiGroup);
    }

    private static void MapContactQueryEndpoints(this WebApplication app, RouteGroupBuilder apiGroup)
    {
        apiGroup.MapGet("/", (ContactApiQueryHandler handler, CancellationToken cancellationToken) => 
                handler.GetAllContacts(cancellationToken))
            .WithName("GetAllContacts");

        apiGroup.MapGet("/{licenseNumber}", (ContactApiQueryHandler handler, string licenseNumber, CancellationToken cancellationToken) => 
                handler.GetContactByRegistrationNumber(licenseNumber, cancellationToken))
            .WithName("GetContactByRegistrationNumber");
    }

    private static void MapContactCommandEndpoints(this WebApplication app, RouteGroupBuilder apiGroup)
    {
        apiGroup.MapPost("/", (ContactApiCommandHandler handler, ContactRequest request, CancellationToken cancellationToken) => 
                handler.CreateContact(request, cancellationToken))
            .WithName("CreateContact");

        apiGroup.MapPut("/{licenseNumber}", (ContactApiCommandHandler handler, string licenseNumber, ContactRequest request, CancellationToken cancellationToken) => 
                handler.UpdateContact(licenseNumber, request, cancellationToken))
            .WithName("UpdateContact");

        apiGroup.MapDelete("/{licenseNumber}", (ContactApiCommandHandler handler, string licenseNumber, CancellationToken cancellationToken) => 
                handler.DeleteContact(licenseNumber, cancellationToken))
            .WithName("DeleteContact");
    }
}