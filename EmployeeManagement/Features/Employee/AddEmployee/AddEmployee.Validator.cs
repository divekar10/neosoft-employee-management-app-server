using EmployeeManagement.Contracts.Employee;
using EmployeeManagement.Shared;
using FluentValidation;

namespace EmployeeManagement.Features.EmployeeFeatures;

public static partial class AddEmployee
{
    public class Validator : AbstractValidator<AddEmployeeRequest>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage("First name is required,")
                .NotEmpty()
                .WithMessage("First name is required,");

            When(x => !string.IsNullOrEmpty(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                .MinimumLength(2)
                .MaximumLength(20);
            });

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage("Last name is required,")
                .NotEmpty()
                .WithMessage("Last name is required,");

            When(x => !string.IsNullOrEmpty(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                .MinimumLength(2)
                .MaximumLength(20);
            });

            RuleFor(x => x.EmailAddress)
                .NotNull()
                .WithMessage("Email Address is required,")
                .NotEmpty()
                .WithMessage("Email Address is required,");

            When(x => !string.IsNullOrEmpty(x.EmailAddress), () =>
            {
                RuleFor(x => x.EmailAddress)
                .MaximumLength(100);
            });

            RuleFor(x => x.MobileNumber)
                .NotNull()
                .WithMessage("Mobile Number is required,")
                .NotEmpty()
                .WithMessage("Mobile Number is required,");

            When(x => !string.IsNullOrEmpty(x.MobileNumber), () =>
            {
                RuleFor(x => x.MobileNumber)
                .MaximumLength(20);
            });

            RuleFor(x => x.PanNumber)
                .NotNull()
                .WithMessage("Pan number is required,")
                .NotEmpty()
                .WithMessage("Pan Number is required,");

            When(x => !string.IsNullOrEmpty(x.PanNumber), () =>
            {
                RuleFor(x => x.PanNumber)
                .MaximumLength(20);
            });


            RuleFor(x => x.DateOfJoinee)
                .NotNull()
                .WithMessage("Date of joinee is required.")
                .NotEmpty()
                .WithMessage("Date of joinee is required.");

            When(x => x.DateOfJoinee != null, () =>
            {
                RuleFor(x => x.DateOfJoinee.Value.Date)
                    .LessThan(DateTime.Now.Date)
                    .WithMessage("Date of joinee should be less than today's date");
            });

            RuleFor(x => x.DateOfBirth)
                .NotNull()
                .WithMessage("Date of birth is required.")
                .NotEmpty()
                .WithMessage("Date of birth is required.");

            When(x => x.DateOfBirth != null, () =>
            {
                RuleFor(x => x.DateOfBirth)
                    .LessThan(DateTime.Now)
                    .WithMessage("Date of birth should be less than today's date");
            });

            When(x => x.DateOfBirth != null, () =>
            {
                RuleFor(x => x.DateOfBirth)
                    .Must(x => Utils.Is18EmployeeYearsOld(x))
                    .WithMessage("Employee should be 18 years old.");
            });

            When(x => !string.IsNullOrEmpty(x.PanNumber), () =>
            {
                RuleFor(x => x.PanNumber)
                    .Matches("[A-Z]{5}[0-9]{4}[A-Z]{1}")
                    .WithMessage("Pan number is not valid.");
            });

            When(x => !string.IsNullOrEmpty(x.PassportNumber), () =>
            {
                RuleFor(x => x.PassportNumber)
                    .Matches("^[a-zA-Z0-9]+$")
                    .WithMessage("Passport number should be alphanumeric.");
            });

            When(x => !string.IsNullOrEmpty(x.EmailAddress), () =>
            {
                RuleFor(x => x.EmailAddress)
                    .Matches("^[\\w\\.-]+@[\\w\\.-]+\\.\\w{2,3}$")
                    .WithMessage("Email address is not valid.");
            });

            When(x => !string.IsNullOrEmpty(x.MobileNumber), () =>
            {
                RuleFor(x => x.MobileNumber)
                    .Matches("^[0-9]+$")
                    .WithMessage("Mobile number should be numeric.");
            });
        }
    }
}
