using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Employee.DeleteEmployee;

public static partial class DeleteEmployee
{
    public class Command : IRequest<Result>
    {
        public int Row_Id { get; set; }
    }
}
