using EmployeeManagement.Database;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.DTOs.EmployeeDto;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Employee.GetEmployees;

public static partial class GetEmployees
{
    internal sealed class Handler : IRequestHandler<Query, Result<PaginatedList<GetEmployeeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedList<GetEmployeeDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<PaginatedList<GetEmployeeDto>>(new(ErrorType.Validation, "Request is null."));
            }

            var employee = await _unitOfWork.EmployeeRepository.GetEmployees(request);


            return employee;
        }
    }
}
