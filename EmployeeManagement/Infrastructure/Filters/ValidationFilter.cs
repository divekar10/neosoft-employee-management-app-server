using System.Net;
using EmployeeManagement.Infrastructure.Model;
using FluentValidation;
using FluentValidation.Results;
using Utilities.Content;

namespace EmployeeManagement.Infrastructure.Filters
{
    public class ValidationFilter<T>(
        ILoggerFactory logger,
        IValidator<T> validator)
        : IEndpointFilter where T : class
    {
        private readonly ILogger _logger = logger.CreateLogger<ValidationFilter<T>>();
        private readonly IValidator<T> _validator = validator;

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validatable = context.Arguments.SingleOrDefault(x => x?.GetType() == typeof(T)) as T;

            if (validatable is null)
            {
                return Results.Json(ValidationFilter<T>.GetFailureResponse());
            }

            var validationResult = await _validator.ValidateAsync(validatable);

            if (!validationResult.IsValid)
            {
                return Results.Json(ValidationFilter<T>.GetFailureResponse(validationResult.Errors));
            }

            return await next(context);
        }

        private static Response GetFailureResponse(IEnumerable<ValidationFailure>? failure = null)
        {
            if (failure is null)
                return new Response
                {
                    statusCode = (int)HttpStatusCode.BadRequest
                };

            return new Response
            {
                statusCode = (int)HttpStatusCode.BadRequest,
                errors = failure?.Select(x => ContentLoader.ReturnLanguageData(x.ErrorMessage, "")).ToList()!
            };
        }
    }
}
