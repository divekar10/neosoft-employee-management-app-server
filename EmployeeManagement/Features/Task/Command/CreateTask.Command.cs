using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Tasks.Command;

public static partial class CreateTask
{
    public class Command : IRequest<Result>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
    }
}
