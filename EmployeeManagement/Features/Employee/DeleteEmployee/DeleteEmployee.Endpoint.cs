using AutoMapper;
using Carter;
using EmployeeManagement.Infrastructure.Extensions;
using MediatR;

namespace EmployeeManagement.Features.Employee.DeleteEmployee;

public static partial class DeleteEmployee
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("employee");

            group.MapDelete("delete/{id}", async (int id, IMapper mapper, ISender sender) =>
            {
                //var command = mapper.Map<DeleteEmployee.Command>(request);

                var result = await sender.Send(new DeleteEmployee.Command() { Row_Id = id });
                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            })/*.AddEndpointFilter<ValidationFilter<DeleteEmployeeRequest>>()*/
            .WithTags("employee");
        }
    }
}
