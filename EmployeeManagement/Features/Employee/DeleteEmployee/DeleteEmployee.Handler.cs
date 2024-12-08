using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;

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
                return Result.Failure(new(ErrorType.Validation, "Request is null"));
            }

            var employee = await _unitOfWork.EmployeeRepository.GetFirstOrDefaultAsync(x => x.Row_Id == request.Row_Id);

            if (employee is null)
            {
                return Result.Failure(new(ErrorType.Validation, "Record not found."));
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
