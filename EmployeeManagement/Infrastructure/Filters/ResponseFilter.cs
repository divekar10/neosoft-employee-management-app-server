using EmployeeManagement.Infrastructure.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;
using System.Text.Json.Serialization;
using Utilities.Content;

namespace EmployeeManagement.Infrastructure.Filters
{
    public class ResponseFilter : IEndpointFilter
    {
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            try
            {
                var result = await next(context);


                // Wrap the result in a common response structure
                var commonResponse = new Response
                {
                    statusCode = (int)HttpStatusCode.OK,
                    message = "success",
                    data = result,
                    errors = []
                };

                // Handle error cases
                if (result is IResult res)
                {
                    if (result?.GetType() == typeof(FileContentHttpResult) ||
                        result?.GetType() == typeof(JsonHttpResult<Response>))
                        return result;


                    var statusCode = res.GetType().GetProperty("StatusCode")?.GetValue(res) as int?;
                    var value = res.GetType().GetProperty("Value")?.GetValue(res);

                    var type = value?.GetType().Name;

                    switch (type?.ToLower())
                    {
                        case "string":
                            value = ContentLoader.ReturnLanguageData(value == null ? "" : value?.ToString()!, "");

                            commonResponse = new Response
                            {
                                statusCode = statusCode ?? (int)HttpStatusCode.OK,
                                message = value?.ToString()!
                            };

                            break;
                        default:
                            commonResponse = new Response
                            {
                                statusCode = statusCode ?? (int)HttpStatusCode.OK,
                                message = statusCode switch
                                {
                                    (int)HttpStatusCode.OK => "success",
                                    (int)HttpStatusCode.BadRequest => "failure",
                                    (int)HttpStatusCode.Unauthorized => "unauthorized",
                                    (int)HttpStatusCode.Forbidden => "access restricted",
                                    _ => "failure"
                                },
                                data = value

                            };
                            break;
                    }
                }

                return Results.Json(commonResponse, new System.Text.Json.JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}
