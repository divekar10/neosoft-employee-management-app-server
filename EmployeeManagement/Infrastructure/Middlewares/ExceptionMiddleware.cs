using System.ComponentModel.DataAnnotations;
using System.Net;
using EmployeeManagement.Infrastructure.Model;
using EmployeeManagement.Shared.Constants;
using Utilities.Content;

namespace EmployeeManagement.Infrastructure.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var exceptionData = GetExceptionDetails(exception, context);
            context.Response.StatusCode = exceptionData.StatusCode;
            await context.Response.WriteAsync(exceptionData.ToString());
        }

        private static ResponseModel GetExceptionDetails(Exception exception, HttpContext context)
        {
            //string errorMessage = string.Empty;

            var model = new ResponseModel();

            switch (exception)
            {
                case UnauthorizedAccessException:

                    model.Message = ContentLoader.ReturnLanguageData("", Convert.ToString(context.Request.Headers[Constants.HEADER_LANGUAG_EFIELD]));
                    model.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case ValidationException:

                    model.Message = ContentLoader.ReturnLanguageData(exception.Message, Convert.ToString(context.Request.Headers[Constants.HEADER_LANGUAG_EFIELD]));
                    model.StatusCode = (int)HttpStatusCode.PreconditionFailed;
                    break;
                default:
                    model.Message = ContentLoader.ReturnLanguageData("MSG500", Convert.ToString(context.Request.Headers[Constants.HEADER_LANGUAG_EFIELD]));
                    model.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            return model;
        }
    }
}
