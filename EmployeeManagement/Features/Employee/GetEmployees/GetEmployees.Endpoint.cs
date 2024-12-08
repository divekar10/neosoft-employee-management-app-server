using AutoMapper;
using Carter;
using EmployeeManagement.Contracts.EmployeeFeature;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Infrastructure.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Features.Employee.GetEmployees;

public static partial class GetEmployees
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("employee");

            group.MapPost("employees", async ([FromBody] GetEmployeesRequest request, IMapper mapper, ISender sender) =>
            {
                var query = mapper.Map<GetEmployees.Query>(request);

                var result = await sender.Send(query);
                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            }).AddEndpointFilter<ValidationFilter<GetEmployeesRequest>>()
            .WithTags("employee");
        }
    }
}
