using AutoMapper;
using Carter;
using EmployeeManagement.Contracts.EmployeeFeature;
using EmployeeManagement.Database.Infrastructure;
using EmployeeManagement.DTOs.EmployeeDto;
using EmployeeManagement.Infrastructure.Extensions;
using EmployeeManagement.Infrastructure.Filters;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Features.EmployeeFeature;

public static class GetEmployee
{
    public class Query : IRequest<Result<GetEmployeeDto>>
    {
        public int Row_Id { get; set; }
    }

    public class GetEmployeeResponse
    {
        public int Row_Id { get; set; }
        public required string EmployeeCode { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string EmailAddress { get; set; }
        public required string MobileNumber { get; set; }
        public required string PanNumber { get; set; }
        public string? PassportNumber { get; set; }
        public string? ProfileImage { get; set; }
        public byte Gender { get; set; }
        public string CountryName { get; set; } = string.Empty;
        public string StateName { get; set; } = string.Empty;
        public string CityName { get; set; } = string.Empty;
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public bool IsActive { get; set; }
    }

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

    internal sealed class Handler : IRequestHandler<Query, Result<GetEmployeeDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<GetEmployeeDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<GetEmployeeDto>(new(ErrorType.Validation, "Request is null."));
            }

            var employee = await _unitOfWork.EmployeeRepository.GetEmployee(request.Row_Id);

            return employee;
        }
    }
}

public class GetEmployeeByRowIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("employee");

        group.MapGet("employee/{id}", async (int id, IMapper mapper, ISender sender) =>
        {
            //var query = mapper.Map<GetEmployee.Query>(new
            //{
            //    Id = id
            //});

            var result = await sender.Send(new GetEmployee.Query
            {
                Row_Id = id
            });
            if (result.IsSuccess)
            {
                return result.ToSuccess();
            }
            else
            {
                return result.ToProblemDetails();
            }
        }).WithTags("employee");
    }
}
