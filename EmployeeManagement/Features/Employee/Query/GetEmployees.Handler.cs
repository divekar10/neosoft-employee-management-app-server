using EmployeeManagement.Database;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.DTOs.EmployeeDto;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.Extensions.Options;

namespace EmployeeManagement.Features.Employee.GetEmployees;

public static partial class GetEmployees
{
    internal sealed class Handler : IRequestHandler<Query, Result<PaginatedList<GetEmployeeDto>>>
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
        public async Task<Result<PaginatedList<GetEmployeeDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<PaginatedList<GetEmployeeDto>>(new(ErrorType.Validation, "Request is null."));
            }

            var employeesResult = await _unitOfWork.EmployeeRepository.GetEmployees(request);

            if (employeesResult.IsSuccess)
            {
                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = 3
                };

                var currentDirectory = Directory.GetCurrentDirectory();
                await Parallel.ForEachAsync(employeesResult.Value!.Items, parallelOptions, async (item, token) =>
                {
                    item.ProfileImage = await Utils.FileToBase64(Path.Combine(currentDirectory, _appSettings.ProfilePicturePath, item.ProfileImage!));
                });


            }
            return employeesResult;
        }
    }
}
