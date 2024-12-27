using FluentValidation;

namespace EmployeeManagement.Features.Tasks.Command;

public static partial class CreateTask
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Task title is required.")
                .NotNull()
                .WithMessage("Task title is required.");

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("Task description is required.")
                .NotNull()
                .WithMessage("Task description is required.");

            RuleFor(x => x.EmployeeId)
                .NotNull()
                .WithMessage("Task employee ID is required.");
        }
    }
}
