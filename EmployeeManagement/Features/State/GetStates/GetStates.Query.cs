using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.State.GetStates;

public static partial class GetStates
{
    public class Query : IRequest<Result<List<Entities.State>>>
    {
        public int CountryId { get; set; }
    }
}
