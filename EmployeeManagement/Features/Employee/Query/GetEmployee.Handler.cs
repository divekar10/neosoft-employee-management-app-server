using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.DTOs.EmployeeDto;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Features.Employee.GetEmployee;

public static partial class GetEmployee
{
    internal sealed class Handler : IRequestHandler<Query, Result<GetEmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppSettings _appSettings;

        public Handler(
            IUnitOfWork unitOfWork,
            IOptions<AppSettings> options)
        {
            _unitOfWork = unitOfWork;
            _appSettings = options.Value;
        }
        public async Task<Result<GetEmployeeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<GetEmployeeDto>(new(ErrorType.Validation, "Request is null."));
            }

            var employee = await _unitOfWork.EmployeeRepository.GetEmployee(request.Row_Id);

            if (employee.IsSuccess)
            {
                employee.Value.ProfileImage = await Utils.FileToBase64(Path.Combine(Directory.GetCurrentDirectory(), 
                                                                       _appSettings.ProfilePicturePath,
                                                                       employee.Value.ProfileImage!));
            }

            return employee;
        }
    }
}
