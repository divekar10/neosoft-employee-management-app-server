using AutoMapper;
using Carter;
using EmployeeManagement.Contracts.Employee;
using EmployeeManagement.Contracts.EmployeeFeature;
using EmployeeManagement.Database;
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

public static class GetEmployees
{

    public class Query : IRequest<Result<PaginatedList<GetEmployeeDto>>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
    }

    public class GetEmployeesResponse
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
        public bool IsActive { get; set; }
    }

    public class Validator : AbstractValidator<GetEmployeesRequest>
    {
        public Validator()
        {
            
        }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<PaginatedList<GetEmployeeDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<PaginatedList<GetEmployeeDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<PaginatedList<GetEmployeeDto>>(new(ErrorType.Validation, "Request is null."));
            }

            var employee = await _unitOfWork.EmployeeRepository.GetEmployees(new GetEmployeesRequest
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                SearchText = request.SearchText,
                SortOrder = request.SortOrder
            });
            

            return employee;
        }
    }
}

public class GetEmployeesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("employee");

        group.MapPost("employees", async ([FromBody] GetEmployeesRequest request, IMapper mapper, ISender sender) =>
        {
            var query = mapper.Map<GetEmployees.Query>(request);

            var result = await sender.Send(query);
            if (result.IsSuccess)
            {
                return result.ToSuccess();
            }
            else
            {
                return result.ToProblemDetails();
            }
        }).AddEndpointFilter<ValidationFilter<GetEmployeesRequest>>()
        .WithTags("employee");
    }
}
