using System.Text.Json;
using AutoMapper;
using Carter;
using EmployeeManagement.Contracts.Employee;
using EmployeeManagement.Infrastructure.Extensions;
using FluentValidation;
using MediatR;

namespace EmployeeManagement.Features.EmployeeFeatures;

public static partial class CreateEmployee
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("employee");

            group.MapPost("add", async (HttpRequest request, IMapper mapper, IValidator<AddEmployeeRequest> validator, ISender sender) =>
            {
                var form = await request.ReadFormAsync();
                var data = form["data"];
                var file = form.Files["file"];

                var deserializeEmployeeData = JsonSerializer.Deserialize<AddEmployeeRequest>(data!);

                if (deserializeEmployeeData is null)
                {
                    throw new ArgumentNullException(nameof(AddEmployeeRequest));
                }

                var validationResult = validator.Validate(deserializeEmployeeData);

                if (!validationResult.IsValid)
                {
                    return ResultExtensions.ToValidationFailure(validationResult);
                }

                var command = new CreateEmployee.Command()
                {
                    AddEmployeeRequest = deserializeEmployeeData,
                    ProfileImage = file!
                };

                if (command is null)
                {
                    throw new ArgumentNullException(nameof(AddEmployeeRequest));
                }

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
            //.AddEndpointFilter<ValidationFilter<AddEmployeeRequest>>()
            .WithTags("employee")
            .DisableAntiforgery();
        }
    }
}