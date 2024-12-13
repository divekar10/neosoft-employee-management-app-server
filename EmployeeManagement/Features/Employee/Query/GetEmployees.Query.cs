using EmployeeManagement.Database;
using EmployeeManagement.DTOs.EmployeeDto;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Employee.GetEmployees;

public static partial class GetEmployees
{
    public class Query : IRequest<Result<PaginatedList<GetEmployeeDto>>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SearchText { get; set; } = string.Empty;
        public string SortBy { get; set; } = string.Empty;
        public string SortOrder { get; set; } = string.Empty;
    }
}
