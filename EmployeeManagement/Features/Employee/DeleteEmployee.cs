using AutoMapper;
using Carter;
using EmployeeManagement.Contracts.EmployeeFeature;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Infrastructure.Filters;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Features.EmployeeFeature;

public static class DeleteEmployee
{
    public class Command : IRequest<Result>
    {
        public int Row_Id { get; set; }
    }

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

public class DeleteEmployeeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("employee");

        group.MapDelete("delete/{id}", async (int id, IMapper mapper, ISender sender) =>
        {
            //var command = mapper.Map<DeleteEmployee.Command>(request);

            var result = await sender.Send(new DeleteEmployee.Command() { Row_Id = id });
            if (result.IsSuccess)
            {
                return result.ToSuccess();
            }
            else
            {
                return result.ToProblemDetails();
            }
        })/*.AddEndpointFilter<ValidationFilter<DeleteEmployeeRequest>>()*/
        .WithTags("employee");
    }
}
