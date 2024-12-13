using EmployeeManagement.Shared.Result;
using MediatR;

namespace EmployeeManagement.Features.Country.GetCountries;

public static partial class GetCountries
{
    public class Query : IRequest<Result<List<Entities.Country>>>
    {

    }
}
