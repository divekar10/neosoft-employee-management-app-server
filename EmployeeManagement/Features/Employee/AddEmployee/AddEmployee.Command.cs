using EmployeeManagement.Contracts.Employee;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.EmployeeFeatures;

public static partial class AddEmployee
{
    public class Command : IRequest<Result>
    {
        public AddEmployeeRequest AddEmployeeRequest { get; set; }
        public IFormFile ProfileImage { get; set; }
    }
}
