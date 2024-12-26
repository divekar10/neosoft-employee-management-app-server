using EmployeeManagement.Entities.BaseEntity;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Common.Generic.Commands
{
    public class UpdateCommand<T> : IRequest<Result<T>>
    {
        public T Entity { get; set; }
        public UpdateCommand(T entity)
        {
            Entity = entity;
        }
    }
}
