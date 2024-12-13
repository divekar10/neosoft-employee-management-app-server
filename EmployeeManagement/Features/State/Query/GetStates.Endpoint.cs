using Carter;
using EmployeeManagement.Infrastructure.Extensions;
using MediatR;

namespace EmployeeManagement.Features.State.GetStates;

public static partial class GetStates
{
    public class Endpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("state");

            group.MapGet("states/{countryId}", async (int countryId, ISender sender) =>
            {
                var result = await sender.Send(new GetStates.Query() { CountryId = countryId });

                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            }).WithTags("state");
        }
    }
}