using Carter;
using EmployeeManagement.Infrastructure.Extensions;
using MediatR;

namespace EmployeeManagement.Features.Country.GetCountries;

public static partial class GetCountries
{
    public class GetContriesEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("country");

            group.MapGet("countries", async (ISender sender) =>
            {
                var result = await sender.Send(new GetCountries.Query());

                if (result.IsSuccess)
                {
                    return result.ToSuccess();
                }
                else
                {
                    return result.ToProblemDetails();
                }
            }).WithTags("country");
        }
    }
}