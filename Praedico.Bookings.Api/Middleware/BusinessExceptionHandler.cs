using System.Net;
using Praedico.Exceptions;

namespace Praedico.Bookings.Api.Middleware;

    public sealed class ExceptionHandlerMiddleware(
        RequestDelegate next,
        IWebHostEnvironment environment,
        ILogger<ExceptionHandlerMiddleware> logger)
    {
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (NotFoundException ex)
            {
                logger.LogWarning(ex, "Not found exception caught");
                await SetIResultResponse(context, Results.NotFound(new { Error = ex.Message }));
            }
            catch (BusinessException ex)
            {
                logger.LogWarning(ex, "Business exception caught");
                await SetIResultResponse(context, Results.BadRequest(new { Error = ex.Message }));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Unhandled exception caught");
                
                if (environment.IsDevelopment() || environment.EnvironmentName.Equals("Local", StringComparison.OrdinalIgnoreCase))
                {
                    await SetIResultResponse(context, Results.StatusCode((int)HttpStatusCode.InternalServerError));
                }
                else
                {
                    throw; // Let the server handle exceptions in non-development environments
                }
            }
        }

        private static async Task SetIResultResponse(HttpContext context, IResult result)
        {
            context.Response.Clear();
            await result.ExecuteAsync(context);
        }
    }
