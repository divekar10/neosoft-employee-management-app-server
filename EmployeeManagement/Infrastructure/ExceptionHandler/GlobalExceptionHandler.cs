using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EmployeeManagement.Infrastructure.ExceptionHandler;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Exception occurred : {Message}", exception.Message);


        var problemDetails = HandleException(exception);
        httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken: cancellationToken);

        return true;
    }

    private static ProblemDetails HandleException(Exception exception)
    {
        return exception switch
        {
            SecurityTokenExpiredException => new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "invalid_token",
                Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1"
            },

            ArgumentNullException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "bad_request",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "server_error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1"
            },
        };
    }
}
