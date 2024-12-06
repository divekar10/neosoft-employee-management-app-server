using System.Text.Json;
using AutoMapper;
using Carter;
using EmployeeManagement.Contracts.Employee;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Entities;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Infrastructure.Filters;
using EmployeeManagement.Shared;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Features.EmployeeFeatures;

public static class AddEmployee
{
    public class Command : IRequest<Result>
    {
        public AddEmployeeRequest AddEmployeeRequest { get; set; }
        public IFormFile ProfileImage { get; set; }
    }

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

    internal sealed class Handler
        : IRequestHandler<Command, Result>
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
                return Result.Failure(new(ErrorType.Validation, "Request is null."));
            }

            if (request.ProfileImage is not null)
            {
                var extensions = new string[2] { ".jpg", ".png" };

                var extention = Path.GetExtension(request.ProfileImage.FileName);

                if (!extensions.Contains(extention))
                {
                    return Result.Failure(new(ErrorType.Validation, "Only .jpg and .png file are allowed."));
                }
            }


            var isMobileNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.MobileNumber == request.AddEmployeeRequest.MobileNumber);

            if (isMobileNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, "Mobile number already exists."));
            }

            var isEmailExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.EmailAddress == request.AddEmployeeRequest.EmailAddress);

            if (isEmailExists)
            {
                return Result.Failure(new(ErrorType.Validation, "Email address already exists."));
            }

            var isPanNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.PanNumber == request.AddEmployeeRequest.PanNumber);

            if (isPanNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, "Pan number already exists."));
            }

            var isPassportNumberExists = await _unitOfWork.EmployeeRepository
                .AnyAsync(x => x.PassportNumber == request.AddEmployeeRequest.PassportNumber);

            if (isPassportNumberExists)
            {
                return Result.Failure(new(ErrorType.Validation, "Passport number already exists."));
            }

            bool isExists = true;

            while (isExists)
            {
                var employeeNumber = string.Format("{0}{1}", "00", Utils.GetRandomNumber());

                var isEmployeeCodeExists = await _unitOfWork.EmployeeRepository.AnyAsync(x => x.EmployeeCode == employeeNumber);

                if (!isEmployeeCodeExists)
                {
                    isExists = false;
                }
            }

            var employee = new Employee()
            {
                FirstName = request.AddEmployeeRequest.FirstName,
                LastName = request.AddEmployeeRequest.LastName,
                EmailAddress = request.AddEmployeeRequest.EmailAddress,
                EmployeeCode = request.AddEmployeeRequest.EmployeeCode,
                MobileNumber = request.AddEmployeeRequest.MobileNumber,
                PanNumber = request.AddEmployeeRequest.PanNumber,
                CityId = request.AddEmployeeRequest.CityId,
                CountryId = request.AddEmployeeRequest.CountryId,
                StateId = request.AddEmployeeRequest.StateId,
                PassportNumber = request.AddEmployeeRequest.PassportNumber,
                Gender = request.AddEmployeeRequest.Gender,
                DateOfBirth = request.AddEmployeeRequest.DateOfBirth,
                DateOfJoinee = request.AddEmployeeRequest.DateOfJoinee,
                IsActive = request.AddEmployeeRequest.IsActive,
                CreatedDate = DateTime.Now,
                ProfileImage = request.ProfileImage != null
                    ? await Utils.SaveFileAsync(request.ProfileImage)
                    : ""
            };

            _unitOfWork.EmployeeRepository.Add(employee);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}

public class AddEmployeeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("employee");

        group.MapPost("add", async (HttpRequest request, IMapper mapper, IValidator<AddEmployeeRequest> validator, ISender sender) =>
        {
            var form = await request.ReadFormAsync();
            var data = form["data"];
            var file = form.Files["file"];

            var validationResult = validator.Validate(JsonSerializer.Deserialize<AddEmployeeRequest>(data));

            if (!validationResult.IsValid)
            {
                return ResultExtensions.ToValidationFailure(validationResult);
            }

            var command = new AddEmployee.Command()
            {
                AddEmployeeRequest = JsonSerializer.Deserialize<AddEmployeeRequest>(data),
                ProfileImage = file
            };

            if (command is null)
            {
                throw new ArgumentNullException(nameof(AddEmployeeRequest));
            }

            var result = await sender.Send(command);

            if (result.IsSuccess)
            {
                return result.ToSuccess();
            }
            else
            {
                return result.ToProblemDetails();
            }
        })
        //.AddEndpointFilter<ValidationFilter<AddEmployeeRequest>>()
        .WithTags("employee")
        .DisableAntiforgery();
    }
}
