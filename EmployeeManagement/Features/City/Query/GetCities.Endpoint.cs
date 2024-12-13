using Carter;
using EmployeeManagement.Infrastructure.Extensions;
using MediatR;

namespace EmployeeManagement.Features.City.GetCities;

public static partial class GetCities
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("city");

            group.MapGet("cities/{stateId}", async (int stateId, ISender sender) =>
            {
                var result = await sender.Send(new GetCities.Query { StateId = stateId });

                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }

            }).WithTags("city");
        }
    }
}
