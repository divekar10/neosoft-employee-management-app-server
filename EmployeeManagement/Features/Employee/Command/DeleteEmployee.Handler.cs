using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Utilities.Content;

namespace EmployeeManagement.Features.Employee.DeleteEmployee;

public static partial class DeleteEmployee
{
    internal sealed class Handler : IRequestHandler<Command, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP103")));
            }

            var employee = await _unitOfWork.EmployeeRepository.GetFirstOrDefaultAsync(x => x.Id == request.Row_Id);

            if (employee is null)
            {
                return Result.Failure(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP104")));
            }

            employee.UpdatedDate = DateTime.Now;
            employee.DeletedDate = DateTime.Now;
            employee.IsDeleted = true;
            employee.IsActive = false;

            _unitOfWork.EmployeeRepository.Update(employee);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
