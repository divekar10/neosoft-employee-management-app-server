using AutoMapper;
using Carter;
using EmployeeManagement.Infrastructure.Extensions;
using MediatR;

namespace EmployeeManagement.Features.Employee.GetEmployee;

public static partial class GetEmployee
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("employee");

            group.MapGet("employee/{id}", async (int id, IMapper mapper, ISender sender) =>
            {
                //var query = mapper.Map<GetEmployee.Query>(new
                //{
                //    Id = id
                //});

                var result = await sender.Send(new GetEmployee.Query
                {
                    Row_Id = id
                });
                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            }).WithTags("employee");
        }
    }
}