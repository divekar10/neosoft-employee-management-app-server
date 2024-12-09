using EmployeeManagement.Contracts.EmployeeFeature;
using FluentValidation;
using Utilities.Content;

namespace EmployeeManagement.Features.Employee.DeleteEmployee;

public static partial class DeleteEmployee
{
    public class Validator : AbstractValidator<DeleteEmployeeRequest>
    {
        public Validator()
        {
            RuleFor(x => x.Row_Id)
                .NotNull()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP218"))
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP218"));
        }
    }
}
