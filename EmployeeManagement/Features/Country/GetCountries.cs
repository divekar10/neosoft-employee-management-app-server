using Carter;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.CountryFeature;

public static class GetCountries
{
    public class Query : IRequest<Result<List<Country>>>
    {

    }

    internal sealed class Handler : IRequestHandler<Query, Result<List<Country>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<Country>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var countries = await _unitOfWork.CountryRepository.GetAllAsync();

            return Result.Success(countries);
        }
    }
}

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
