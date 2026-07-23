using TaskFlow.Api.Contracts;

namespace TaskFlow.Api.Middleware;

public sealed class ErrorHandlingMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> logger;
    private readonly RequestDelegate next;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Unhandled API exception.");

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse(
                "An unexpected error occurred.",
                new[] { "The request could not be completed." });

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}
