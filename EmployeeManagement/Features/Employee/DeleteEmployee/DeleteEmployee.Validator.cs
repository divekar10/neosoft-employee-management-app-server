using EmployeeManagement.Contracts.EmployeeFeature;
using FluentValidation;

namespace EmployeeManagement.Features.Employee.DeleteEmployee;

public static partial class DeleteEmployee
{
    public class Validator : AbstractValidator<DeleteEmployeeRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Row_Id)
                .NotNull()
                .WithMessage("Employee ID is required.")
                .NotEmpty()
                .WithMessage("Employee ID is required.");
        }
    }
}
