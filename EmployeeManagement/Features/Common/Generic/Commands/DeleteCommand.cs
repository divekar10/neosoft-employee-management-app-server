using EmployeeManagement.Entities.BaseEntity;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Common.Generic.Commands
{
    public class DeleteCommand<T> : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public DeleteCommand(int id)
        {
            Id = id;
        }
    }
}
