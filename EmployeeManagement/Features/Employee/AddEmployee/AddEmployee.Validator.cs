using EmployeeManagement.Contracts.Employee;
using EmployeeManagement.Shared;
using FluentValidation;
using Utilities.Content;

namespace EmployeeManagement.Features.EmployeeFeatures;

public static partial class AddEmployee
{
    public class Validator : AbstractValidator<AddEmployeeRequest>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName)
                .NotNull()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP205"))
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP205"));

            When(x => !string.IsNullOrEmpty(x.FirstName), () =>
            {
                RuleFor(x => x.FirstName)
                .MinimumLength(2)
                .MaximumLength(20);
            });

            RuleFor(x => x.LastName)
                .NotNull()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP206"))
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP206"));

            When(x => !string.IsNullOrEmpty(x.LastName), () =>
            {
                RuleFor(x => x.LastName)
                .MinimumLength(2)
                .MaximumLength(20);
            });

            RuleFor(x => x.EmailAddress)
                .NotNull()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP207"))
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP207"));

            When(x => !string.IsNullOrEmpty(x.EmailAddress), () =>
            {
                RuleFor(x => x.EmailAddress)
                .MaximumLength(100);
            });

            RuleFor(x => x.MobileNumber)
                .NotNull()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP208"))
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP208"));

            When(x => !string.IsNullOrEmpty(x.MobileNumber), () =>
            {
                RuleFor(x => x.MobileNumber)
                .MaximumLength(20);
            });

            RuleFor(x => x.PanNumber)
                .NotNull()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP209"))
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP209"));

            When(x => !string.IsNullOrEmpty(x.PanNumber), () =>
            {
                RuleFor(x => x.PanNumber)
                .MaximumLength(20);
            });


            RuleFor(x => x.DateOfJoinee)
                .NotNull()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP210"))
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP210"));

            When(x => x.DateOfJoinee != null, () =>
            {
                RuleFor(x => x.DateOfJoinee.Value.Date)
                    .LessThan(DateTime.Now.Date)
                    .WithMessage(ContentLoader.ReturnLanguageData("EMP211"));
            });

            RuleFor(x => x.DateOfBirth)
                .NotNull()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP212"))
                .NotEmpty()
                .WithMessage(ContentLoader.ReturnLanguageData("EMP212"));

            //When(x => x.DateOfBirth != null, () =>
            //{
            //    RuleFor(x => x.DateOfBirth)
            //        .LessThan(DateTime.Now)
            //        .WithMessage("Date of birth should be less than today's date");
            //});

            When(x => x.DateOfBirth != null, () =>
            {
                RuleFor(x => x.DateOfBirth)
                    .Must(x => Utils.Is18EmployeeYearsOld(x))
                    .WithMessage(ContentLoader.ReturnLanguageData("EMP213"));
            });

            When(x => !string.IsNullOrEmpty(x.PanNumber), () =>
            {
                RuleFor(x => x.PanNumber)
                    .Matches("[A-Z]{5}[0-9]{4}[A-Z]{1}")
                    .WithMessage(ContentLoader.ReturnLanguageData("EMP214"));
            });

            When(x => !string.IsNullOrEmpty(x.PassportNumber), () =>
            {
                RuleFor(x => x.PassportNumber)
                    .Matches("^[A-Z][1-9]\\d\\s?\\d{4}[1-9]$")
                    .WithMessage(ContentLoader.ReturnLanguageData("EMP215"));
            });

            When(x => !string.IsNullOrEmpty(x.EmailAddress), () =>
            {
                RuleFor(x => x.EmailAddress)
                    .Matches("^[\\w\\.-]+@[\\w\\.-]+\\.\\w{2,3}$")
                    .WithMessage(ContentLoader.ReturnLanguageData("EMP216"));
            });

            When(x => !string.IsNullOrEmpty(x.MobileNumber), () =>
            {
                RuleFor(x => x.MobileNumber)
                    .Matches("^[0-9]+$")
                    .WithMessage(ContentLoader.ReturnLanguageData("EMP217"));
            });
        }
    }
}
