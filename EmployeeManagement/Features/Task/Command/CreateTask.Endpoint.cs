using AutoMapper;
using Carter;
using EmployeeManagement.Contracts.Tasks;
using EmployeeManagement.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Features.Tasks.Command;

public static partial class CreateTask
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("tasks");

            group.MapPost("add", async ([FromBody] CreateTaskRequest request, ISender sender, IMapper mapper) =>
            {
                var command = mapper.Map<CreateTask.Command>(request);

                var result = await sender.Send(command);

                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            });
        }
    }
}