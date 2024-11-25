using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Praedico.Bookings.Infrastructure.Diagnostics;

public class DiagnosticsObserver(ILogger<DiagnosticsObserver> logger) : IObserver<DiagnosticListener>
{
    public void OnNext(DiagnosticListener listener)
    {
        if (listener.Name == "Microsoft.EntityFrameworkCore")
        {
            listener.Subscribe(new EfDiagnosticsObserver(logger)!);
        }
    }

    public void OnError(Exception error)
    {
        logger.LogError(error, "EF Diagnostics Listener encountered an error.");
    }

    public void OnCompleted()
    {
        logger.LogInformation("EF Diagnostics Listener completed.");
    }
}

public class EfDiagnosticsObserver(ILogger<DiagnosticsObserver> logger) : IObserver<KeyValuePair<string, object>>
{
    public void OnNext(KeyValuePair<string, object> eventData)
    {
        logger.LogInformation("EF Event: {Key}, Data: {Value}", eventData.Key, eventData.Value);
    }

    public void OnError(Exception error)
    {
        logger.LogError(error, "EF Diagnostics Event encountered an error.");
    }

    public void OnCompleted()
    {
        logger.LogInformation("EF Diagnostics Event processing completed.");
    }
}