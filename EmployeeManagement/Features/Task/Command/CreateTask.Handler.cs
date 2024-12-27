using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Tasks.Command;

public static partial class CreateTask
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
                return Result.Failure<Result>(new(ErrorType.Validation, ""));
            }

            await _unitOfWork.TasksRepository.AddAsync(new Entities.Tasks
            {
                Title = request.Title,
                Description = request.Description,
                EmployeeId = request.EmployeeId,
                TaskStatus = nameof(Shared.Enum.TaskStatus.Pending)
            });

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
