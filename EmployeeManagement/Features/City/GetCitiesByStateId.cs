using Carter;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Features.CityFeature;

public static class GetCitiesByStateId
{
    public class Query : IRequest<Result<List<City>>>
    {
        public int StateId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<List<City>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<City>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<List<City>>(new(ErrorType.Validation, "Request is null"));
            }

            var cities = await _unitOfWork.CityRepository
                .GetAllAsync(x => x.StateId == request.StateId)
                .ToListAsync();

            return Result.Success(cities);
        }
    }
}

public class GetCitiesByStateIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("city");

        group.MapGet("cities/{stateId}", async (int stateId, ISender sender) =>
        {
            var result = await sender.Send(new GetCitiesByStateId.Query { StateId = stateId });

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
