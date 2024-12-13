using System.Text.Json;
using AutoMapper;
using Carter;
using EmployeeManagement.Contracts.EmployeeFeature;
using EmployeeManagement.Infrastructure.Extensions;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Features.Employee.UpdateEmployee;

public static partial class UpdateEmployee
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("employee");

            group.MapPut("update", async (HttpRequest request, IMapper mapper, IValidator<UpdateEmployeeRequest> validator, ISender sender) =>
            {
                var form = await request.ReadFormAsync();
                var data = form["data"];
                var file = form.Files["file"];

                var deserialzeEmployeeData = JsonSerializer.Deserialize<UpdateEmployeeRequest>(data!);

                if (deserialzeEmployeeData is null)
                {
                    throw new ArgumentNullException(nameof(deserialzeEmployeeData));
                }

                var validationResult = validator.Validate(deserialzeEmployeeData);

                if (!validationResult.IsValid)
                {
                    return ResultExtensions.ToValidationFailure(validationResult);
                }


                var command = new UpdateEmployee.Command()
                {
                    UpdateEmployeeRequest = deserialzeEmployeeData,
                    File = file!
                };

                var result = await sender.Send(command);

                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            })
            .WithTags("employee")
            .DisableAntiforgery();
        }
    }
}