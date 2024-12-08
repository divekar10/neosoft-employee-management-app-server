using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Features.City.GetCities;

public static partial class GetCities
{
    internal sealed class Handler : IRequestHandler<Query, Result<List<Entities.City>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<Entities.City>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<List<Entities.City>>(new(ErrorType.Validation, "Request is null"));
            }

            var cities = await _unitOfWork.CityRepository
                .GetAllAsync(x => x.StateId == request.StateId)
                .ToListAsync();

            return Result.Success(cities);
        }
    }
}
