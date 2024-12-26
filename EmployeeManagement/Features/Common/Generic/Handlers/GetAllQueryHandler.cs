using EmployeeManagement.Database;
using EmployeeManagement.Entities.BaseEntity;
using EmployeeManagement.Features.Common.Generic.Queries;
using EmployeeManagement.Shared.Enum;
using EmployeeManagement.Shared.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Utilities.Content;

namespace EmployeeManagement.Features.Common.Generic.Handlers
{
    public class GetAllQueryHandler<T> : IRequestHandler<GetAllQuery<T>, Result<IEnumerable<T>>>
            where T : class
    {
        private readonly EmployeeDbContext _context;
        public GetAllQueryHandler(EmployeeDbContext context) => _context = context;

        public async Task<Result<IEnumerable<T>>> Handle(GetAllQuery<T> request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<IEnumerable<T>>(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP103")));
            }

            var result = await _context.Set<T>().ToListAsync(cancellationToken);
            if(result is not null && result.Count < 0)
            {
                return Result.Failure<IEnumerable<T>>(new(ErrorType.NotFound, ContentLoader.ReturnLanguageData("EMP104")));
            }

            return Result.Success<IEnumerable<T>>(result);
        }
    }
}
