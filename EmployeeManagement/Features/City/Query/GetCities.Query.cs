using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.City.GetCities;

public static partial class GetCities
{
    public class Query : IRequest<Result<List<Entities.City>>>
    {
        public int StateId { get; set; }
    }
}
