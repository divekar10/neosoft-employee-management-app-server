using EmployeeManagement.Entities.BaseEntity;
using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Common.Generic.Queries
{
    public class GetByIdQuery<T> : IRequest<Result<T>>
    {
        public int Id { get; set; }
        public GetByIdQuery(int id)
        {
            Id = id;
        }
    }
}
