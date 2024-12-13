using EmployeeManagement.DTOs.EmployeeDto;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Employee.GetEmployee;

public static partial class GetEmployee
{
    public class Query : IRequest<Result<GetEmployeeDto>>
    {
        public int Row_Id { get; set; }
    }
}
