using EmployeeManagement.Entities.BaseEntity;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Common.Generic.Queries
{
    public class GetAllQuery<T> : IRequest<Result<IEnumerable<T>>> where T : class, IEntity
    {
    }
}
