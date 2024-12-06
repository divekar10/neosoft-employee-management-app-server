using System.Net;
using System.Text.Json.Serialization;
using EmployeeManagement.Infrastructure.Model;
using EmployeeManagement.Shared.Result;
using FluentValidation.Results;

namespace EmployeeManagement.Infrastructure.Extensions
{
    public static class ResultExtensions
    {
        public static IResult ToSuccess(this Result result, string? message = default)
        {
            if (result.IsFailure)
            {
                throw new InvalidOperationException("Can't convert failure result to success.");
            }

            var commonResponse = new Response
            {
                statusCode = (int)HttpStatusCode.OK,
                message = message ?? "success"
            };

            return Results.Json(commonResponse, new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
        }

        public static IResult ToSuccess<T>(this Result<T> result, string? message = default)
        {
            if (result.IsFailure)
            {
                throw new InvalidOperationException("Can't convert failure result to success.");
            }

            var commonResponse = new Response
            {
                statusCode = (int)HttpStatusCode.OK,
                message = message ?? "success",
                data = result.Value,
            };

            return Results.Json(commonResponse, new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            });
        }

        //public static IResult ToFile(this Result<FileResponse> result)
        //{
        //    if (result.IsFailure)
        //    {
        //        throw new InvalidOperationException("Can't convert failure result to success.");
        //    }

        //    return Results.File(
        //        result.Value!.fileBytes,
        //        result.Value.ContentTye,
        //        result.Value.FileName);
        //}

        public static IResult ToNotFound(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Can't convert success result to problem.");
            }

            return Results.Problem
                (
                statusCode: StatusCodes.Status404NotFound,
                title: "Bad Request",
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4",
                extensions: new Dictionary<string, object?>
                {
                    { "errors", new[] { result.Error } }
                });
        }

        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Can't convert success result to problem.");
            }

            return Results.Problem
                (
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                extensions: new Dictionary<string, object?>
                {
                    { "errors", new[] { result.Error.Discription } }
                });
        }

        public static IResult ToValidationFailure(ValidationResult validationResult)
        {
            return Results.Problem
                (
                statusCode: StatusCodes.Status400BadRequest,
                title: "Bad Request",
                type: "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                extensions: new Dictionary<string, object?>
                {
                    { "errors", new[] { validationResult.Errors.Select(x => x.ErrorMessage) } }
                });
        }
    }
}
