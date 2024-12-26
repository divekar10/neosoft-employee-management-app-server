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
    public class GetByIdQueryHandler<T> : IRequestHandler<GetByIdQuery<T>, Result<T>>
        where T : class
    {
        private readonly EmployeeDbContext _context;
        public GetByIdQueryHandler(EmployeeDbContext context) => _context = context;

        public async Task<Result<T>> Handle(GetByIdQuery<T> request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                return Result.Failure<T>(new(ErrorType.Validation, ContentLoader.ReturnLanguageData("EMP103")));
            }

            var result = await _context.Set<T>().FindAsync(new object[] { request.Id }, cancellationToken);

            if(result is null)
            {
                return Result.Failure<T>(new(ErrorType.NotFound, ContentLoader.ReturnLanguageData("EMP104")));
            }

            return Result.Success(result!);
        }
    }
}
