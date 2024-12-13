using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Features.State.GetStates;

public static partial class GetStates
{
    internal sealed class Handler : IRequestHandler<Query, Result<List<Entities.State>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<Entities.State>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<List<Entities.State>>(new(ErrorType.Validation, "Request is null"));
            }

            var states = await _unitOfWork.StateRepository
                .GetAllAsync(x => x.CountryId == request.CountryId)
                .ToListAsync();

            return Result.Success(states);
        }
    }
}
