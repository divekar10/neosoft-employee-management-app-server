using EmployeeManagement.Entities.BaseEntity;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Common.Generic.Commands
{
    public class CreateCommand<T> : IRequest<Result<T>>
    {
        public T Entity { get; }

        public CreateCommand(T entity)
        {
            Entity = entity;
        }
    }
}
