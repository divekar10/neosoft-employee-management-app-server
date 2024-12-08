using EmployeeManagement.Contracts.EmployeeFeature;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Employee.UpdateEmployee;

public static partial class UpdateEmployee
{
    public class Command : IRequest<Result>
    {
        public UpdateEmployeeRequest UpdateEmployeeRequest { get; set; }
        public IFormFile File { get; set; }
    }
}
