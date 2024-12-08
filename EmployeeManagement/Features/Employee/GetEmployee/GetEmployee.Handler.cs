using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.DTOs.EmployeeDto;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Employee.GetEmployee;

public static partial class GetEmployee
{
    internal sealed class Handler : IRequestHandler<Query, Result<GetEmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<GetEmployeeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<GetEmployeeDto>(new(ErrorType.Validation, "Request is null."));
            }

            var employee = await _unitOfWork.EmployeeRepository.GetEmployee(request.Row_Id);

            return employee;
        }
    }
}
