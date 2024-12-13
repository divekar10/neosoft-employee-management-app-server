using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Country.GetCountries;

public static partial class GetCountries
{
    internal sealed class Handler : IRequestHandler<Query, Result<List<Entities.Country>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<List<Entities.Country>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var countries = await _unitOfWork.CountryRepository.GetAllAsync();

            return Result.Success(countries);
        }
    }
}
