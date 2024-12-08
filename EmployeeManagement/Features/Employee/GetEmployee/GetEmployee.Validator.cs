using EmployeeManagement.Contracts.EmployeeFeature;
using FluentValidation;

namespace EmployeeManagement.Features.Employee.GetEmployee;

public static partial class GetEmployee
{
    public class Validator : AbstractValidator<GetEmployeeByRowIdRequest>
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
