using Carter;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Features.StateFeature;

public static class GetStatesByCountryId
{
    public class Query : IRequest<Result<List<State>>>
    {
        public int CountryId { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<List<State>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<State>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<List<State>>(new(ErrorType.Validation, "Request is null"));
            }

            var states = await _unitOfWork.StateRepository
                .GetAllAsync(x => x.CountryId == request.CountryId)
                .ToListAsync();

            return Result.Success(states);
        }
    }
}

public class GetStatesByCountryIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("state");

        group.MapGet("states/{countryId}", async (int countryId, ISender sender) =>
        {
            var result = await sender.Send(new GetStatesByCountryId.Query() { CountryId = countryId});

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
